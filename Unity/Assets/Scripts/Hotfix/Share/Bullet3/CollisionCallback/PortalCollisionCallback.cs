using BulletSharp;

namespace ET
{
    [FriendOf(typeof(PortalComponent))]
    public class PortalCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other)
        {
        }

        public override void CollisionCallbackStay(CollisionObject self, CollisionObject other)
        {
            B3CollisionComponent collisionA = self.UserObject as B3CollisionComponent;
            LSUnit unitA = collisionA.GetParent<LSUnit>();
            string tagA = unitA.Tag;
            if (!tagA.StartsWith(Tag.Cast))
            {
                Log.Warning($"Portal{unitA.Id}的Tag异常{tagA}");
                return;
            }

            // 如果碰撞到的B是友方玩家
            B3CollisionComponent collisionB = other.UserObject as B3CollisionComponent;
            if (collisionB == null) return;
            LSUnit unitB = collisionB.GetParent<LSUnit>();
            string tagB = unitB.Tag;
            if (!tagB.StartsWith(Tag.Player)) return;
            if (!Tag.IsFriend(tagA, tagB)) return;

            // V
            if (unitB.GetComponent<LSFInputComponent>().Input.Button != 118) return;

            collisionB.MoveTo(unitA.GetComponent<PortalComponent>().Other.ToVector());
        }

        public override void CollisionCallbackExit(CollisionObject self, CollisionObject other)
        {

        }

        public override void CollisionTestStart(CollisionObject self)
        {
            
        }

        public override void CollisionTestFinish(CollisionObject self)
        {

        }
    }
}