using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET
{
    [FriendOf(typeof(ActionComponent))]
    public class ArmageddonInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            dataModifierComponent.Add(new Default_Speed_ConstantModifier() { Value = 1 });

            B3CollisionComponent collision = owner.GetComponent<B3CollisionComponent>();
            RigidBody body = collision.Collision as RigidBody;
            actionComponent.Args.Add("G", body.Gravity);
            body.Gravity = Vector3.Zero;
            collision.MoveTo(Vector3.UnitY * 10000);
            
            EventSystem.Instance.Publish(unit.IScene as LSWorld, new ArmageddonStart() { Unit = unit });
        }
    }
    [FriendOfAttribute(typeof(ET.ActionComponent))]
    public class ArmageddonDisableActionHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return !actionComponent.Args.ContainsKey("Casting");
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            ActionComponent ownerActionComponent = owner.GetComponent<ActionComponent>();
            foreach (ActionConfig actionConfig in ownerActionComponent.Configs)
            {
                ownerActionComponent.Actives[actionConfig] = false;
            }
        }
    }
    [FriendOf(typeof(ActionComponent))]
    public class ArmageddonMoveHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            LSInput input = owner.GetComponent<LSFInputComponent>().Input;
            if (TSMath.Abs(input.V.x) < 0.1f && TSMath.Abs(input.V.y) < 0.1f)
            {
                return false;
            }

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            LSInput input = owner.GetComponent<LSFInputComponent>().Input;
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();

            TSMatrix matrix = TSMath.RotationMatrix(unit.HeadRotation, unit.Rotation);
            TSVector offset = matrix * new TSVector(input.V.x, 0, input.V.y) * dataModifierComponent.Get(DataModifierType.Speed);
            // 因为这里还没有B3CollisionComponent, 直接移动Unit就行
            unit.Position += offset;
        }
    }
    [FriendOfAttribute(typeof(ET.ActionComponent))]
    public class ArmageddonCastHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            if (actionComponent.Args.ContainsKey("Casting")) return false;
            
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            LSInput input = owner.GetComponent<LSFInputComponent>().Input;
            if (input.Button != 113) return false;

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            unit.AddComponent<B3CollisionComponent, int>(14);

            actionComponent.Args.Add("Casting", true);
        }
    }

    [FriendOfAttribute(typeof(ET.ActionComponent))]
    public class ArmageddonEndHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return actionComponent.Args.ContainsKey("Casting");
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;

            B3CollisionComponent collision = owner.GetComponent<B3CollisionComponent>();
            RigidBody body = collision.Collision as RigidBody;
            body.Gravity = (Vector3)actionComponent.Args["G"];

            collision.MoveTo(unit.Position.ToBullet());
            
            ActionComponent ownerActionComponent = owner.GetComponent<ActionComponent>();
            foreach (ActionConfig actionConfig in ownerActionComponent.Configs)
            {
                ownerActionComponent.Actives[actionConfig] = true;
            }

            LSWorld world = owner.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            EventSystem.Instance.Publish(world , new ArmageddonEnd() { UnitId = room.PlayerId });
        }
    }
}