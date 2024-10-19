using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET.Client
{
    public class SkeletonMoveHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;
            if (TSMath.Abs(input.V.x) < 0.1f && TSMath.Abs(input.V.y) < 0.1f)
            {
                EventSystem.Instance.Publish(actionComponent.IScene as LSWorld, new UnitChangeMoveSpeed() { Unit = unit, Speed = 0 });
                return false;
            }
            
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            
            long speed = unit.GetComponent<DataModifierComponent>()[DataModifierType.Speed];
            EventSystem.Instance.Publish(actionComponent.IScene as LSWorld, new UnitChangeMoveSpeed() { Unit = unit, Speed = speed });
        }
    }
}