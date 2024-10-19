using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET.Server
{
    [FriendOf(typeof(ActionComponent))]
    public class SkeletonInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            
            long id = timerComponent.AddTimer(dataModifierComponent.Get(DataModifierType.ECD));
            actionComponent.Args.Add("ERate", id);
            
            id = timerComponent.AddTimer(dataModifierComponent.Get(DataModifierType.QCD));
            actionComponent.Args.Add("QRate", id);
            
            id = timerComponent.AddTimer(dataModifierComponent.Get(DataModifierType.CCD));
            actionComponent.Args.Add("CRate", id);
        }
    }

    [FriendOf(typeof(BuffComponent))]
    [FriendOf(typeof(DataModifierComponent))]
    [FriendOf(typeof(ActionComponent))]
    [FriendOf(typeof(CheckOnGroundComponent))]
    public class SkeletonHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            /*LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;
            return input.V != TSVector2.zero || input.Button != 0;*/
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;

            /*JumpHandler(actionComponent, input, unit);
            MoveHandler(actionComponent, input, unit);
            LookHandler(actionComponent, input, unit);
            E_CastHandler(actionComponent, input, unit);
            Q_CastHandler(actionComponent, input, unit);
            C_CastHandler(actionComponent, input, unit);
            ChangeBulletCountHandler(actionComponent, input, unit);*/
        }

        private static void MoveHandler(ActionComponent actionComponent, LSInput input, LSUnit unit)
        {
            RigidBody body = unit.GetComponent<B3CollisionComponent>().Collision as RigidBody;

            TSVector2 v2 = input.V;
            if (TSMath.Abs(v2.x) < 0.1f && TSMath.Abs(v2.y) < 0.1f)
            {
                body.LinearVelocity = new Vector3(0, body.LinearVelocity.Y, 0);
                return;
            }

            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            
            TSMatrix matrix = TSMath.RotationMatrix(unit.HeadRotation, unit.Rotation);
            TSVector offset = matrix * new TSVector(v2.x, 0, v2.y) * dataModifierComponent.Get(DataModifierType.Speed);
            offset.y = body.LinearVelocity.Y;
            body.LinearVelocity = offset.ToBullet();
        }

        private static void LookHandler(ActionComponent actionComponent, LSInput input, LSUnit unit)
        {
            TSVector2 v2 = input.Look;
            if (TSMath.Abs(v2.x) < 0.01f && TSMath.Abs(v2.y) < 0.01f)
            {
                return;
            }

            unit.Rotation += 20 * v2.x;
            unit.HeadRotation += 20 * v2.y;
        }

        private static void E_CastHandler(ActionComponent actionComponent, LSInput input, LSUnit unit)
        {
            if (input.Button != 101) return;

            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();

            int needBulletCount = 10;

            // 检查子弹数
            long bulletCount = dataModifierComponent.Get(DataModifierType.BulletCount);
            if (bulletCount < needBulletCount)
            {
                input.Button = 114;
                ChangeBulletCountHandler(actionComponent, input, unit);
                return;
            }
            
            // 检查技能间隔
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            if (!timerComponent.CheckTimeOut((long)actionComponent.Args["ERate"], true)) return;

            // 释放技能
            CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1001);

            // 减子弹数
            dataModifierComponent.Add(new Default_BulletCount_ConstantModifier() { Value = -needBulletCount }, true);
            bulletCount = dataModifierComponent.Get(DataModifierType.BulletCount);
            if (bulletCount < needBulletCount)
            {
                input.Button = 114;
                ChangeBulletCountHandler(actionComponent, input, unit);
                return;
            }
        }

        private static void Q_CastHandler(ActionComponent actionComponent, LSInput input, LSUnit unit)
        {
            if (input.Button != 113) return;
            
            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            
            // 检查技能间隔
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            if (!timerComponent.CheckTimeOut((long)actionComponent.Args["QRate"], true)) return;
            
            // 释放技能
            CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1002);
        }

        private static void C_CastHandler(ActionComponent actionComponent, LSInput input, LSUnit unit)
        {
            if (input.Button != 99) return;
            
            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            
            // 检查技能间隔
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            if (!timerComponent.CheckTimeOut((long)actionComponent.Args["CRate"], true)) return;
            
            // 释放技能
            BuffComponent buffComponent = unit.GetComponent<BuffComponent>();
            buffComponent.Creat(1002);
        }

        private static void JumpHandler(ActionComponent actionComponent, LSInput input, LSUnit unit)
        {
            if (input.Jump == false) return;
            
            CheckOnGroundComponent checkOnGroundComponent = unit.GetComponent<CheckOnGroundComponent>();
            if (checkOnGroundComponent == null || !checkOnGroundComponent.OnGround) return;

            RigidBody body = unit.GetComponent<B3CollisionComponent>().Collision as RigidBody;
            body.LinearVelocity = Vector3.UnitY * 20;
        }

        private static void ChangeBulletCountHandler(ActionComponent actionComponent, LSInput input, LSUnit unit)
        {
            if (input.Button != 114) return;

            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            var values = dataModifierComponent.GetValues(DataModifierType.BulletCount);
            dataModifierComponent.Add(new Default_BulletCount_ConstantModifier() { Value = values.Item2 - values.Item1 }, true);
        }
    }
}