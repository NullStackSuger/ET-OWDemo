using BulletSharp;

namespace ET
{
    public class FireBallCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other)
        {
            B3CollisionComponent collision = self.UserObject as B3CollisionComponent;
            LSUnit castUnit = collision.GetParent<LSUnit>();
        }

        public override void CollisionCallbackStay(CollisionObject self, CollisionObject other)
        {
        }

        public override void CollisionCallbackExit(CollisionObject self, CollisionObject other)
        {
        }
    }
}