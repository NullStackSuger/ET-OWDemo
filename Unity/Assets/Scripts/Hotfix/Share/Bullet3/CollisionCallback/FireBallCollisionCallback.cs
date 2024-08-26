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

        public override void CollisionTestStart()
        {
           
        }

        public override void CollisionTestFinish()
        {
            // 比如仓鼠球的E, 本来是一次范围检测, 但Bullet中我没找到范围检测, 就用碰撞检测代替, 可以在这里直接移除碰撞(触发器)
            // 虽然我们在CollisionCallbackEnter调用NotifyToRemove功能是一样, 但比如想退出时创建一些特效, 如果还是用原来的方法, 就会创建很多个特效
        }
    }
}