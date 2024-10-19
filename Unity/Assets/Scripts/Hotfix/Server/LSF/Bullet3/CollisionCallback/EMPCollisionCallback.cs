using BulletSharp;

namespace ET.Server
{
    public class EMPCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other, PersistentManifold persistentManifold)
        {
            B3CollisionComponent collisionA = self.UserObject as B3CollisionComponent;
            LSUnit unitA = collisionA.GetParent<LSUnit>();
            string tagA = unitA.Tag;
            if (!tagA.StartsWith(Tag.Cast))
            {
                Log.Warning($"EMP{unitA.Id}的Tag异常{tagA}");
                return;
            }

            // 如果碰撞到的B是敌方玩家/护盾
            B3CollisionComponent collisionB = other.UserObject as B3CollisionComponent;
            if (collisionB == null) return;
            
            LSUnit unitB = collisionB.GetParent<LSUnit>();
            string tagB = unitB.Tag;
            //if (Tag.IsFriend(tagA, tagB)) return;

            if (tagB.StartsWith(Tag.Shield))
            {
                LSUnit ownerB = unitB.Owner;
                DataModifierComponent modifierComponentB = ownerB.GetComponent<DataModifierComponent>();
                long shield = modifierComponentB.Get(DataModifierType.Shield);
                modifierComponentB.Add(new Default_Shield_ConstantModifier() { Value = -shield });
            }
            else if (tagB.StartsWith(Tag.Player))
            {
                BuffComponent buffComponent = unitB.GetComponent<BuffComponent>();
                buffComponent.Creat(1003);
            }
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