using System;
using BulletSharp;

namespace ET
{
    public class FireBallCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other)
        {
            B3CollisionComponent collisionA = self.UserObject as B3CollisionComponent;
            LSUnit unitA = collisionA.GetParent<LSUnit>();
            string tagA = unitA.Tag;
            if (!tagA.StartsWith(Tag.Cast))
            {
                Log.Warning($"FireBall{unitA.Id}的Tag异常{tagA}");
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
                LSUnit ownerB = unitB.Owner;
                DataModifierComponent modifierComponentB = ownerB.GetComponent<DataModifierComponent>();
                long atk = modifierComponentA.Get(DataModifierType.Atk);
                DataModifierHelper.Shield(atk, modifierComponentB, DataModifierType.Shield);
                
                castComponentA.Remove(unitA);
            }
            else if (tagB.StartsWith(Tag.Player))
            {
                DataModifierComponent modifierComponentB = unitB.GetComponent<DataModifierComponent>();
            
                Log.Warning($"伤害");
                long atk = modifierComponentA.Get(DataModifierType.Atk);
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
            // 比如仓鼠球的E, 本来是一次范围检测, 但Bullet中我没找到范围检测, 就用碰撞检测代替, 可以在这里直接移除碰撞(触发器)
            // 虽然我们在CollisionCallbackEnter调用NotifyToRemove功能是一样, 但比如想退出时创建一些特效, 如果还是用原来的方法, 就会创建很多个特效
        }
    }
}