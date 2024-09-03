using BulletSharp;

namespace ET
{
    public class FireBallCollisionCallback : ACollisionCallback
    {
        public override void CollisionCallbackEnter(CollisionObject self, CollisionObject other)
        {
            // 获取A数值
            B3CollisionComponent collisionA = self.UserObject as B3CollisionComponent;
            LSUnit unitA = collisionA.GetParent<LSUnit>();
            string tagA = unitA.Tag;
            LSUnit ownerA = unitA.Owner;
            DataModifierComponent modifierComponentA = ownerA.GetComponent<DataModifierComponent>();
            CastComponent castComponentA = ownerA.GetComponent<CastComponent>();

            // 获取B数值
            B3CollisionComponent collisionB = other.UserObject as B3CollisionComponent;
            if (collisionB == null)
            {
                castComponentA.Remove(unitA);
                return;
            }
            LSUnit unitB = collisionB.GetParent<LSUnit>();
            if (unitB == null)
            {
                // TODO 按理说这里可以不用检查空, 但是确实会报空. 还没找到原因
                //Log.Warning($"{collisionB.Id} {collisionB.InstanceId}");
                castComponentA.Remove(unitA);
                return;
            }
            string tagB = unitB.Tag;
            if (tagA == tagB && (tagB == TeamTag.TeamA || tagB == TeamTag.TeamB))
            {
                castComponentA.Remove(unitA);
                return;
            }
            LSUnit ownerB = unitB.Owner;
            DataModifierComponent modifierComponentB = ownerB.GetComponent<DataModifierComponent>();
            
            // 处理战斗公式
            // Add - [FinalAdd - (Atk) / (1 + FinalPct)] / (1 + Pct)
            long atk = modifierComponentA.Get(DataModifierType.Atk);
            DataModifierHelper.DefaultBattle(atk, modifierComponentB, DataModifierType.Hp);
            
            // 这里可以添加一些如距离衰减等FinalXXX操作
            
            castComponentA.Remove(unitA);
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