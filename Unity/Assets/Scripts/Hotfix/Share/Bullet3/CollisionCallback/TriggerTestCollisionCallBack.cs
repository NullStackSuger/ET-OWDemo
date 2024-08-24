using BulletSharp;

namespace ET
{

    public class TriggerTestCollisionCallBack : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other)
        {
            Log.Warning($"开始触发");
        }

        public override void CollisionCallbackStay(CollisionObject self, CollisionObject other)
        {
            Log.Warning($"触发中");
        }

        public override void CollisionCallbackExit(CollisionObject self, CollisionObject other)
        {
            Log.Warning($"结束触发");
        }
    }
}