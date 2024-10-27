using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace ET.Client
{
    public static class AnimatorState
    {
        public const string None = "None";
        public const string Idle = "Idle";
        public const string Move = "Move";
        public const string Cast = "Cast";
        public const string Jump = "Jump";
        public const string Shoot = "Shoot";
    }
    
    [ComponentOf(typeof(LSFUnitView))]
    public class AnimancerComponent : Entity, IAwake<string>
    {
        public Dictionary<string, AnimancerTransitionAssetBase> ClipPool = new();
        /// <summary>
        /// 运行时对应State对应的Clip
        /// key: state value: clipName
        /// </summary>
        public Dictionary<string, string> RuntimeClipPool = new();
        
        /// <summary>
        /// 对应Layer上的AnimatorState
        /// key: layerName value: state
        /// </summary>
        public Dictionary<string, string> LayerPeekState = new();
        /// <summary>
        /// 对应Layer的index
        /// key: layerName value: layerIndex
        /// </summary>
        public Dictionary<string, int> Masks = new();
        /// <summary>
        /// 对应Layer的权重, 因为有些问题没找到, 导致每次Play之后会设置权重为1
        /// key: layerName value: weight
        /// </summary>
        public Dictionary<string, float> LayerWeight = new();
        
        public Animancer.AnimancerComponent Animancer;

        public AnimancerTransitionAssetBase this[string stateName]
        {
            get
            {
                return this.ClipPool[this.RuntimeClipPool[stateName]];
            }
            set
            {
                string name = value.name;
                this.ClipPool[name] = value;
                this.RuntimeClipPool[stateName] = name;
            }
        }
    }
}