using BulletSharp;

namespace ET
{
    public class BounceCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other, PersistentManifold persistentManifold)
        {
            B3CollisionComponent collisionA = self.UserObject as B3CollisionComponent;
            LSUnit unitA = collisionA.GetParent<LSUnit>();
            string tagA = unitA.Tag;
            if (!tagA.StartsWith(Tag.Cast))
            {
                Log.Warning($"Bounce{unitA.Id}的Tag异常{tagA}");
                return;
            }
            
            // 如果碰撞到的B是敌方技能
            B3CollisionComponent collisionB = other.UserObject as B3CollisionComponent;
            // 碰撞到建筑
            if (collisionB == null)
            {
                return;
            }
            
            LSUnit unitB = collisionB.GetParent<LSUnit>();
            string tagB = unitB.Tag;
            if (Tag.IsFriend(tagA, tagB)) return;
            if (!tagB.StartsWith(Tag.Cast)) return;
            other.InterpolationLinearVelocity *= -1;
            unitB.Tag = $"{Tag.Cast}_{unitA.Tag.Substring(unitA.Tag.Length - 1, 1)}";
        }

        public override void CollisionCallbackStay(CollisionObject self, CollisionObject other, PersistentManifold persistentManifold)
        {
        
        }

        public override void CollisionCallbackExit(CollisionObject self, CollisionObject other, PersistentManifold persistentManifold)
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