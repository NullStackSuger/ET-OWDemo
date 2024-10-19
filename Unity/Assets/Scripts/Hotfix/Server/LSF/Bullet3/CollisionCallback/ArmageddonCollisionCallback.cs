using BulletSharp;

namespace ET.Server
{

    public class ArmageddonCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other, PersistentManifold persistentManifold)
        {
            B3CollisionComponent collisionA = self.UserObject as B3CollisionComponent;
            LSUnit unitA = collisionA.GetParent<LSUnit>();
            string tagA = unitA.Tag;
            if (!tagA.StartsWith(Tag.CastPreview))
            {
                Log.Warning($"FireBall{unitA.Id}的Tag异常{tagA}");
                return;
            }
            LSUnit ownerA = unitA.Owner;
            DataModifierComponent modifierComponentA = ownerA.GetComponent<DataModifierComponent>();
            CastComponent castComponentA = ownerA.GetComponent<CastComponent>();
            
            // 如果碰撞到的B是敌方玩家
            B3CollisionComponent collisionB = other.UserObject as B3CollisionComponent;
            // 碰撞到建筑
            if (collisionB == null)
            {
                castComponentA.Remove(unitA);
                return;
            }
            
            LSUnit unitB = collisionB.GetParent<LSUnit>();
            string tagB = unitB.Tag;
            if (Tag.IsFriend(tagA, tagB)) return;
            if (!tagB.StartsWith(Tag.Player)) return;
            
            DataModifierComponent modifierComponentB = unitB.GetComponent<DataModifierComponent>();
            
            long atk = modifierComponentA.Get(DataModifierType.Atk);
            DataModifierHelper.DefaultBattle(atk, modifierComponentB, DataModifierType.Hp);
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

            LSUnit ownerA = unitA.Owner;
            ownerA.GetComponent<CastComponent>().Remove(unitA);
        }
    }
}