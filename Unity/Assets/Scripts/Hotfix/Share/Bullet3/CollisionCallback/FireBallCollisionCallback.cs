using BulletSharp;

namespace ET
{
    public class FireBallCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other)
        {
            B3CollisionComponent collision = self.UserObject as B3CollisionComponent;
            LSUnit castUnit = collision.GetParent<LSUnit>();
            LSUnit owner = castUnit.Owner;
            CastComponent castComponent = owner.GetComponent<CastComponent>();
            castComponent.Remove(castUnit);
        }

        public override void CollisionCallbackStay(CollisionObject self, CollisionObject other)
        {
        }

        public override void CollisionCallbackExit(CollisionObject self, CollisionObject other)
        {
        }
    }
}