using System;
using System.Linq;

namespace ET
{
    [EntitySystemOf(typeof(ActionComponent))]
    [LSEntitySystemOf(typeof(ActionComponent))]
    [FriendOf(typeof(ActionComponent))]
    public static partial class ActionComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ActionComponent self, int group)
        {
            self.Group = group;
            
            var groupConfigs = ActionConfigCategory.Instance[self.Group];
            foreach (var config in groupConfigs.Values)
            {
                self.Configs.Add(config);
                self.Actives.Add(config, true);
            }
        }

        [EntitySystem]
        private static void Destroy(this ActionComponent self)
        {
            self.Configs.Clear();
            self.Actives.Clear();
            self.Args.Clear();
        }

        [LSEntitySystem]
        private static void LSUpdate(this ActionComponent self)
        {
            foreach (ActionConfig config in self.Configs)
            {
                AActionHandler handler = ActionDispatcherComponent.Instance[config.Name];
                if (handler == null)
                {
                    Log.Error($"Can not find handler {config.Name}");
                    continue;
                }

                self.Actives.TryAdd(config, true);

                // 未通过检查
                if (!handler.Check(self, config)) continue;
                
                // 没有被激活
                if (!self.Actives[config]) continue;
                
                // 执行
                handler.Update(self, config);

                // 这里运行之后就return 不循环了 所以可以直接Remove
                if (config.RunningType == "Once")
                {
                    self.Actives.Remove(config);
                    self.Configs.Remove(config);
                }
                
                if (config.IsOnly == 1)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}