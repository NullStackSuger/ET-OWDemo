using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(Cast))]
    [FriendOf(typeof(Cast))]
    public static partial class CastSystem
    {
        [EntitySystem]
        private static void Awake(this Cast self, int configId)
        {
            CastConfig config = CastConfigCategory.Instance.Get(configId);
            self.Awake(configId, new TSVector(config.X, config.Y, config.Z));
        }

        [EntitySystem]
        private static void Awake(this Cast self, int configId, TSVector offset)
        {
            LSUnit player = self.GetParent<CastComponent>().GetParent<LSUnit>();
            CastConfig config = CastConfigCategory.Instance.Get(configId);

            // 创建Unit
            LSUnitComponent unitComponent = (self.IScene as Entity).GetComponent<LSUnitComponent>();
            LSUnit castUnit = unitComponent.Creat(self.Id);
            castUnit.Owner = player;

            self.ConfigId = configId;
            self.Unit = castUnit;

            // 获取Player坐标系到World坐标系矩阵
            TSMatrix matrix = TSMath.RotationMatrix(player.HeadRotation, player.Rotation);
            castUnit.Position = player.Position + matrix * offset;
            castUnit.Rotation = player.Rotation;
            castUnit.HeadRotation = player.HeadRotation;
            
            castUnit.Tag = (config.Tag == ""? Tag.Cast : config.Tag) + $"_{player.Tag.Substring(player.Tag.Length - 1, 1)}";
            
            // 添加行为机
            castUnit.AddComponent<ActionComponent, int>(config.ActionGroup);
            castUnit.AddComponent<CastComponent>();
            castUnit.AddComponent<BuffComponent>();
            
            // 发布事件: 创建技能
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitUseCast() { Owner = player, Cast = self, Name = config.Name });
        }

        [EntitySystem]
        private static void Destroy(this Cast self)
        {
            // 发布事件: 销毁技能
            LSUnit player = self.GetParent<CastComponent>().GetParent<LSUnit>();
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitRemoveCast() { Owner = player, Cast = self });
            
            // 移除CastUnit
            LSUnitComponent unitComponent = player.GetParent<LSUnitComponent>();
            // TODO 这里不会立刻Remove Unit 不确定会不会有什么问题
            unitComponent.WaitToRemove(self.Id);
        }
    }
}