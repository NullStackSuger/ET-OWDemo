using BulletSharp;

namespace ET
{
    public class SkeletonCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other)
        {
            //Log.Warning($"开始碰撞");
        }

        public override void CollisionCallbackStay(CollisionObject self, CollisionObject other)
        {
            //Log.Warning($"碰撞");
        }

        public override void CollisionCallbackExit(CollisionObject self, CollisionObject other)
        {
            //Log.Warning($"结束碰撞");
        }

        public override void CollisionTestStart()
        {
            
        }

        public override void CollisionTestFinish()
        {
            
        }
    }
}