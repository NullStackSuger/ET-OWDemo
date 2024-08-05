using BulletSharp;

namespace ET
{
    public class FireBallCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other)
        {
            Log.Warning($"Enter {(self.UserObject as B3CollisionComponent) == null} {(other.UserObject as B3CollisionComponent) == null}");
        }

        public override void CollisionCallbackStay(CollisionObject self, CollisionObject other)
        {
            Log.Warning("Stay");
        }

        public override void CollisionCallbackExit(CollisionObject self, CollisionObject other)
        {
            Log.Warning("Exit");
        }
    }
}