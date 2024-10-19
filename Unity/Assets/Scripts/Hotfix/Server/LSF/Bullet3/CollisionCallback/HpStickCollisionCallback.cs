using BulletSharp;

namespace ET.Server
{
    public class HpStickCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other, PersistentManifold persistentManifold)
        {
            B3CollisionComponent collisionA = self.UserObject as B3CollisionComponent;
            LSUnit unitA = collisionA.GetParent<LSUnit>();
            string tagA = unitA.Tag;
            if (!tagA.StartsWith(Tag.Cast))
            {
                Log.Warning($"HpStick{unitA.Id}的Tag异常{tagA}");
                return;
            }
            LSUnit ownerA = unitA.Owner;
            DataModifierComponent modifierComponentA = ownerA.GetComponent<DataModifierComponent>();
            
            // 如果碰撞到的B是友方玩家
            B3CollisionComponent collisionB = other.UserObject as B3CollisionComponent;
            if (collisionB == null) return;
            LSUnit unitB = collisionB.GetParent<LSUnit>();
            string tagB = unitB.Tag;
            if (!tagB.StartsWith(Tag.Player)) return;
            if (!Tag.IsFriend(tagA, tagB)) return;
            DataModifierComponent modifierComponentB = unitB.GetComponent<DataModifierComponent>();
            
            Log.Warning($"治疗");
            long value = modifierComponentA.Get(DataModifierType.ENumeric);
            DataModifierHelper.HpStick(value, modifierComponentB, DataModifierType.Hp);
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
            // 移除碰撞
            B3CollisionComponent collisionA = self.UserObject as B3CollisionComponent;
            if (collisionA == null) return;
            LSUnit unitA = collisionA.GetParent<LSUnit>();
            unitA.RemoveComponent<B3CollisionComponent>();
        }
    }
}