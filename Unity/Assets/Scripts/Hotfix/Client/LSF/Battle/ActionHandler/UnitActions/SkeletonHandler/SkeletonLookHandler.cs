using TrueSync;

namespace ET.Client
{
    public class SkeletonLookHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;
            
            if (TSMath.Abs(input.Look.x) < 0.1f && TSMath.Abs(input.Look.y) < 0.2f)
            {
                return false;
            }

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;
            
            //unit.Rotation += 10 * input.Look.x;
            //unit.HeadRotation += 5 * input.Look.y;
        }
    }
}