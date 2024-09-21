using BulletSharp;

namespace ET
{

    public class FortBulletCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other)
        {
            Log.Warning($"碰撞");
            B3CollisionComponent collisionA = self.UserObject as B3CollisionComponent;
            LSUnit unitA = collisionA.GetParent<LSUnit>();
            string tagA = unitA.Tag;
            if (!tagA.StartsWith(Tag.Cast))
            {
                Log.Warning($"FortBullet{unitA.Id}的Tag异常{tagA}");
                return;
            }
            LSUnit ownerA = unitA.Owner;
            DataModifierComponent modifierComponentA = ownerA.GetComponent<DataModifierComponent>();
            CastComponent castComponentA = ownerA.GetComponent<CastComponent>();
            
            // 如果碰撞到的B是敌方玩家/护盾
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
            
            if (tagB.StartsWith(Tag.Shield))
            {
                Log.Warning($"攻击护盾");
                LSUnit ownerB = unitB.Owner;
                DataModifierComponent modifierComponentB = ownerB.GetComponent<DataModifierComponent>();
                long atk = modifierComponentA.Get(DataModifierType.FortNumeric);
                DataModifierHelper.Shield(atk, modifierComponentB, DataModifierType.Shield);

                castComponentA.Remove(unitA);
            }
            else if (tagB.StartsWith(Tag.Player))
            {
                Log.Warning($"碰到玩家");
                DataModifierComponent modifierComponentB = unitB.GetComponent<DataModifierComponent>();

                long atk = modifierComponentA.Get(DataModifierType.FortNumeric);
                DataModifierHelper.DefaultBattle(atk, modifierComponentB, DataModifierType.Hp);
                castComponentA.Remove(unitA);
            }
        }

        public override void CollisionCallbackStay(CollisionObject self, CollisionObject other)
        {
            
        }

        public override void CollisionCallbackExit(CollisionObject self, CollisionObject other)
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