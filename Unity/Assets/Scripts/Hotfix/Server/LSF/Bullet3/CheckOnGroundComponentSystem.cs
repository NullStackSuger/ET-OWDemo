using BulletSharp;
using BulletSharp.Math;

namespace ET.Server
{
    [EntitySystemOf(typeof(CheckOnGroundComponent))]
    [LSEntitySystemOf(typeof(CheckOnGroundComponent))]
    [FriendOf(typeof(CheckOnGroundComponent))]
    public static partial class CheckOnGroundComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CheckOnGroundComponent self)
        {
            
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this CheckOnGroundComponent self)
        {
            LSUnit unit = self.GetParent<LSUnit>();
            Vector3 pos = unit.Position.ToBullet();

            LSWorld world = self.IScene as LSWorld;
            B3WorldComponent worldComponent = world.GetComponent<B3WorldComponent>();
            Vector3 from = pos + Vector3.UnitY * 0.1f; // 需要向上偏移点, 要不会检测不到, Bullet真垃圾
            Vector3 to = pos + Vector3.UnitY * -0.2f;
            self.OnGround = worldComponent.RayTestFirst(from, to, out CollisionObject co);
        }
    }
}