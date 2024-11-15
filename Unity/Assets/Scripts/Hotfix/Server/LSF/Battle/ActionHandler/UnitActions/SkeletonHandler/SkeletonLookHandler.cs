using TrueSync;

namespace ET.Server
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
            
            // 2倍灵敏度 * 基础旋转 * 输入
            unit.Rotation += 2 * 10 * input.Look.x;
            unit.HeadRotation += 2 * 7 * input.Look.y;
            
        }
    }
}