using System.Numerics;
using BulletSharp;
using BulletSharp.Math;

namespace ET
{
    [EntitySystemOf(typeof(CastComponent))]
    [FriendOf(typeof(CastComponent))]
    public static partial class CastComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CastComponent self)
        {

        }
        
        public static Cast Creat(this CastComponent self, int configId)
        {
            LSUnit player = self.GetParent<LSUnit>();
            
            LSUnitComponent unitComponent = (self.IScene as Entity).GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.Creat();
            
            Cast cast = self.AddChild<Cast, int, LSUnit>(configId, unit);
            
            // 这个必须要在创建Cast消息发送后再移动
            unit.Position = player.Position;
            unit.Rotation = player.Rotation;
            unit.Forward = player.Forward;
            
            // 这个必须在设置unit位置后
            CastConfig castConfig = CastConfigCategory.Instance.Get(configId);
            using var rb = RigidBodyConfigCategory.Instance.Clone(castConfig.RigidBody);
            RigidBodyConfig rigidBodyConfig = RigidBodyConfigCategory.Instance.Get(castConfig.RigidBody);
            ACollisionCallback callback = CollisionCallbackDispatcherComponent.Instance[rigidBodyConfig.Callback];
            unit.AddComponent<B3CollisionComponent, RigidBodyConstructionInfo, ACollisionCallback>(rb, callback);

            self.Casts.Add(cast);
            
            return cast;
        }
        
        public static Cast Creat(this CastComponent self, int configId, long castUnitId)
        {
            LSUnitComponent unitComponent = (self.IScene as Entity).GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.Creat(castUnitId);
            LSUnit player = self.GetParent<LSUnit>();
            
            Cast cast = self.AddChild<Cast, int, LSUnit>(configId, unit);
            
            unit.Position = player.Position;
            unit.Rotation = player.Rotation;
            unit.Forward = player.Forward;

            self.Casts.Add(cast);
            
            return cast;
        }

        public static void Remove(this CastComponent self, Cast cast)
        {
            self.Casts.Remove(cast);
            self.RemoveChild(cast.Id);
        }
    }
}