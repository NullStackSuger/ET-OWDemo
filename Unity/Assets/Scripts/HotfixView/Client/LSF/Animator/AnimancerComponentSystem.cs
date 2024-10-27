using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(AnimancerComponent))]
    [FriendOf(typeof(AnimancerComponent))]
    [FriendOf(typeof(LSFUnitView))]
    public static partial class AnimancerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AnimancerComponent self, string name)
        {
            // 构建Animancer
            self.Animancer = self.GetParent<LSFUnitView>().GameObject.GetComponent<Animancer.AnimancerComponent>();

            // 加载动画资源
            ResourcesLoaderComponent resourcesLoader = self.Root().GetComponent<ResourcesLoaderComponent>();
            string assetsName = $"Assets/Bundles/Unit/{name}";
            GameObject bundleGameObject = resourcesLoader.LoadAssetSync<GameObject>(assetsName);

            List<string> states = new();
            List<string> masks = new();
            foreach (var kv in AnimatorStateConfigCategory.Instance.GetAll())
            {
                AnimatorStateConfig config = kv.Value;

                if (!states.Contains(config.Name)) states.Add(config.Name);
                if (config.Layer != "All" && !masks.Contains(config.Layer)) masks.Add(config.Layer);
            }

            // 设置Mask
            // 设置LayerPeekState
            float weight = 1 / (float)masks.Count;
            foreach (string maskName in masks)
            {
                AvatarMask mask = bundleGameObject.Get<AvatarMask>(maskName);
                var layer = self.Animancer.Layers.Add();
                layer.SetMask(mask);
                layer.Weight = weight;
                self.LayerWeight.Add(maskName, weight);
                self.Masks.Add(maskName, self.Animancer.Layers.Count - 1);
                self.LayerPeekState.Add(maskName, AnimatorState.None);
            }

            // 设置ClipPool
            // 设置RuntimeClipPool
            foreach (var kv in bundleGameObject.GetAll<AnimancerTransitionAssetBase>())
            {
                // 相等的是Clip 不相同是State
                if (!kv.Key.StartsWith("AnimationClip_"))
                {
                    self.RuntimeClipPool.Add(kv.Key, kv.Value.name);
                }

                if (!self.ClipPool.ContainsKey(kv.Value.name))
                {
                    self.ClipPool.Add(kv.Value.name, kv.Value);
                }
            }
            
            
            /*foreach (var kv in self.LayerPeekState)
            {
                Log.Warning($"LayerPeekState: {kv}");
            }
            
            foreach (var kv in self.ClipPool)
            {
                Log.Warning($"ClipPool: {kv}");
            }

            foreach (var kv in self.RuntimeClipPool)
            {
                Log.Warning($"RuntimeClipPool: {kv}");
            }

            foreach (var kv in self.Masks)
            {
                Log.Warning($"Masks: {kv}");
            }*/
        }

        public static void Play(this AnimancerComponent self, string state)
        {
            // 获取正在播放的动画的数据
            AnimatorStateConfig newConfig = AnimatorStateConfigCategory.Instance.GetByName(state);

            foreach (string layerName in GetLayers(self, newConfig.Layer))
            {
                // 判断ID(优先级)
                string peekState = self.LayerPeekState[layerName];
                if (peekState != AnimatorState.None)
                {
                    // 获取相同layer的之前的数据
                    AnimatorStateConfig oldConfig = AnimatorStateConfigCategory.Instance.GetByName(peekState);
                    if (oldConfig.Name == state) continue;
                    if (oldConfig.BreakUnderAble == 0 && newConfig.Id < oldConfig.Id) continue;
                }

                // 播放新动画
                // 循环播放的
                if (newConfig.PlayOnEnd == newConfig.Name)
                {
                    // 1.第一次播放Idle动画时States中不存在, 会正常添加, 第2次时存在了, 会包裹成ClipState(Idle), 然后字典中不存在ClipState(Idle), 就会添加ClipState
                    // 2.点击移动过快会出现2个Idle, 是因为开启了FromStart, https://kybernetik.com.au/animancer/docs/manual/blending/fading/modes/
                    AnimancerLayer layer = self.Animancer.Layers[self.Masks[layerName]];
                    AnimancerState clip = layer.Play(self[state], newConfig.FadeDuration, FadeMode.FromStart);
                    layer.Weight = self.LayerWeight[layerName];
                    // TODO Weight设置有问题, 播放动画会使Layer权重为1
                }
                else
                {
                    AnimancerLayer layer = self.Animancer.Layers[self.Masks[layerName]];
                    AnimancerState clip = layer.Play(self[state], newConfig.FadeDuration);
                    layer.Weight = self.LayerWeight[layerName];
                    OnEndHandler(clip, self, newConfig.PlayOnEnd);
                }

                // 更新LayerPeekState
                self.LayerPeekState[layerName] = state;
            }
        }

        public static void Play(this AnimancerComponent self, string state, float x)
        {
            // 获取正在播放的动画的数据
            AnimatorStateConfig newConfig = AnimatorStateConfigCategory.Instance.GetByName(state);

            foreach (string layerName in GetLayers(self, newConfig.Layer))
            {
                // 判断ID(优先级)
                string peekState = self.LayerPeekState[layerName];
                if (peekState != AnimatorState.None)
                {
                    // 获取相同layer的之前的数据
                    AnimatorStateConfig oldConfig = AnimatorStateConfigCategory.Instance.GetByName(peekState);
                    // 重复调用只改变参数
                    if (oldConfig.Name == state)
                    {
                        AnimancerLayer layer = self.Animancer.Layers[self.Masks[layerName]];
                        var key = self[state].Key;
                        LinearMixerState linearClip = layer.GetState(ref key) as LinearMixerState;
                        // 这里可以考虑Clamp
                        linearClip.Parameter = x;
                        continue;
                    }
                    if (oldConfig.BreakUnderAble == 0 && newConfig.Id < oldConfig.Id) continue;
                }
                
                // 播放新动画
                // 循环播放的
                if (newConfig.PlayOnEnd == newConfig.Name)
                {
                    // 1.第一次播放Idle动画时States中不存在, 会正常添加, 第2次时存在了, 会包裹成ClipState(Idle), 然后字典中不存在ClipState(Idle), 就会添加ClipState
                    // 2.点击移动过快会出现2个Idle, 是因为开启了FromStart, https://kybernetik.com.au/animancer/docs/manual/blending/fading/modes/
                    AnimancerLayer layer = self.Animancer.Layers[self.Masks[layerName]];
                    AnimancerState clip = layer.Play(self[state], newConfig.FadeDuration, FadeMode.FromStart);
                    layer.Weight = self.LayerWeight[layerName];
                    // TODO Weight设置有问题, 播放动画会使Layer权重为1
                }
                else
                {
                    AnimancerLayer layer = self.Animancer.Layers[self.Masks[layerName]];
                    AnimancerState clip = layer.Play(self[state], newConfig.FadeDuration);
                    layer.Weight = self.LayerWeight[layerName];
                    OnEndHandler(clip, self, newConfig.PlayOnEnd);
                }

                // 更新LayerPeekState
                self.LayerPeekState[layerName] = state;
            }
        }
        
        public static void Play(this AnimancerComponent self, string state, Vector2 v)
        {
            // 获取正在播放的动画的数据
            AnimatorStateConfig newConfig = AnimatorStateConfigCategory.Instance.GetByName(state);

            foreach (string layerName in GetLayers(self, newConfig.Layer))
            {
                // 判断ID(优先级)
                string peekState = self.LayerPeekState[layerName];
                if (peekState != AnimatorState.None)
                {
                    // 获取相同layer的之前的数据
                    AnimatorStateConfig oldConfig = AnimatorStateConfigCategory.Instance.GetByName(peekState);
                    // 重复调用只改变参数
                    if (oldConfig.Name == state)
                    {
                        AnimancerLayer layer = self.Animancer.Layers[self.Masks[layerName]];
                        var key = self[state].Key;
                        MixerState<Vector2> dirClip = layer.GetState(ref key) as MixerState<Vector2>;
                        // 这里可以考虑Normalize
                        dirClip.Parameter = v;
                        continue;
                    }
                    if (oldConfig.BreakUnderAble == 0 && newConfig.Id < oldConfig.Id) continue;
                }
                
                // 播放新动画
                // 循环播放的
                if (newConfig.PlayOnEnd == newConfig.Name)
                {
                    // 1.第一次播放Idle动画时States中不存在, 会正常添加, 第2次时存在了, 会包裹成ClipState(Idle), 然后字典中不存在ClipState(Idle), 就会添加ClipState
                    // 2.点击移动过快会出现2个Idle, 是因为开启了FromStart, https://kybernetik.com.au/animancer/docs/manual/blending/fading/modes/
                    AnimancerLayer layer = self.Animancer.Layers[self.Masks[layerName]];
                    AnimancerState clip = layer.Play(self[state], newConfig.FadeDuration, FadeMode.FromStart);
                    layer.Weight = self.LayerWeight[layerName];
                    // TODO Weight设置有问题, 播放动画会使Layer权重为1
                }
                else
                {
                    AnimancerLayer layer = self.Animancer.Layers[self.Masks[layerName]];
                    AnimancerState clip = layer.Play(self[state], newConfig.FadeDuration);
                    layer.Weight = self.LayerWeight[layerName];
                    OnEndHandler(clip, self, newConfig.PlayOnEnd);
                }

                // 更新LayerPeekState
                self.LayerPeekState[layerName] = state;
            }
        }

        private static void OnEndHandler(AnimancerState clip, AnimancerComponent self, string playOnEnd)
        {
            clip.Events.OnEnd = OnEnd;
            
            // 结束回调
            void OnEnd()
            {
                clip.Time = 0;
                if (playOnEnd == "") return;
                self.Play(playOnEnd);
            }
        }
        
        // 获取Layer
        private static List<string> GetLayers(AnimancerComponent self, string name)
        {
            List<string> layers = new();

            // 指定所有Layer
            if (name == "All")
            {
                layers.AddRange(self.Masks.Keys);
                return layers;
            }

            // 指定多个Layer
            if (name.Contains(','))
            {
                foreach (string layerName in name.Split(','))
                {
                    layers.Add(layerName);
                }
                return layers;
            }

            // 单个Layer
            layers.Add(name);

            return layers;
        }
    }
}