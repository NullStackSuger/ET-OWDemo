using BulletSharp;

namespace ET
{
    [EntitySystemOf(typeof(Cast))]
    [FriendOf(typeof(Cast))]
    public static partial class CastSystem
    {
        [EntitySystem]
        private static void Awake(this Cast self, int configId)
        {
            LSUnit player = self.GetParent<CastComponent>().GetParent<LSUnit>();
            CastConfig config = CastConfigCategory.Instance.Get(configId);
            
            // 创建Unit
            LSUnitComponent unitComponent = (self.IScene as Entity).GetComponent<LSUnitComponent>();
            LSUnit castUnit = unitComponent.Creat();
            castUnit.Owner = self;
            
            self.ConfigId = configId;
            self.Unit = castUnit;

            // 添加行为机
            self.AddComponent<ActionComponent, int>(config.ActionGroup);

            // 发布事件: 创建技能
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitUseCast() { Unit = player, Cast = self });
            
            // 这个必须要在创建Cast消息发送后再移动
            castUnit.Position = player.Position;
            castUnit.Rotation = player.Rotation;
            
            // 添加碰撞
            if (config.RigidBody == 0) return;
            using var rb = RigidBodyConfigCategory.Instance.Clone(config.RigidBody);
            RigidBodyConfig rigidBodyConfig = RigidBodyConfigCategory.Instance.Get(config.RigidBody);
            ACollisionCallback callback = CollisionCallbackDispatcherComponent.Instance[rigidBodyConfig.Callback];
            castUnit.AddComponent<B3CollisionComponent, RigidBodyConstructionInfo, ACollisionCallback>(rb, callback);
        }

        [EntitySystem]
        private static void Awake(this Cast self, int configId, long castUnitId)
        {
            LSUnit player = self.GetParent<CastComponent>().GetParent<LSUnit>();
            CastConfig config = CastConfigCategory.Instance.Get(configId);
            
            // 创建Unit
            LSUnitComponent unitComponent = (self.IScene as Entity).GetComponent<LSUnitComponent>();
            LSUnit castUnit = unitComponent.Creat(castUnitId);
            castUnit.Owner = self;
            
            self.ConfigId = configId;
            self.Unit = castUnit;
            
            // 添加行为机
            self.AddComponent<ActionComponent, int>(config.ActionGroup);
            
            // 发布事件: 创建技能
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitUseCast() { Unit = player, Cast = self });
            
            // 这个必须要在创建Cast消息发送后再移动
            castUnit.Position = player.Position;
            castUnit.Rotation = player.Rotation;
            
            // 添加碰撞
            if (config.RigidBody == 0) return;
            using var rb = RigidBodyConfigCategory.Instance.Clone(config.RigidBody);
            RigidBodyConfig rigidBodyConfig = RigidBodyConfigCategory.Instance.Get(config.RigidBody);
            ACollisionCallback callback = CollisionCallbackDispatcherComponent.Instance[rigidBodyConfig.Callback];
            castUnit.AddComponent<B3CollisionComponent, RigidBodyConstructionInfo, ACollisionCallback>(rb, callback);
        }

        [EntitySystem]
        private static void Destroy(this Cast self)
        {
            // 发布事件: 销毁技能
            LSUnit player = self.GetParent<CastComponent>().GetParent<LSUnit>();
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitRemoveCast() { Unit = player, Cast = self });
            
            // 移除CastUnit
            LSUnitComponent unitComponent = player.GetParent<LSUnitComponent>();
            LSUnit castUnit = self.Unit;
            if (castUnit == null) return;
            unitComponent.RemoveChild(castUnit.Id);
        }
    }
}