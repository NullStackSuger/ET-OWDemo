using TrueSync;

namespace ET.Client
{
    [AnimatorHandler(AnimatorType.Skeleton)]
    public class SkeletonAnimatorHandler : AAnimatorHandler
    {
        public override void Update(LSFAnimatorComponent animatorComponent, LSInput input)
        {
            float speed = 6f;
            if (input.V != TSVector2.zero)
            {
                animatorComponent.SetFloat("Speed", speed);
            }
            else
            {
                animatorComponent.SetFloat("Speed", 0);
            }
        }
    }
}