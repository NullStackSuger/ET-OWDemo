using TrueSync;

namespace ET.Client
{
    [AnimatorHandler(AnimatorType.Skeleton)]
    [FriendOf(typeof(ActionComponent))]
    [FriendOf(typeof(CheckOnGroundComponent))]
    public class SkeletonAnimatorHandler : AAnimatorHandler
    {
        public override void Update(LSFAnimatorComponent animatorComponent, LSUnit unit)
        {
            var inputComponent = unit.GetComponent<LSFInputComponent>();
            if (inputComponent == null) Log.Error("未获取到InputComponent");

            LSInput input = inputComponent.Input;

            float speed = 6f;
            if (input.V != TSVector2.zero)
            {
                animatorComponent.SetFloat("Speed", speed);
            }
            else
            {
                animatorComponent.SetFloat("Speed", 0);
            }

            CheckOnGroundComponent checkOnGroundComponent = unit.GetComponent<CheckOnGroundComponent>();
            animatorComponent.SetBool("OnGround", checkOnGroundComponent == null || checkOnGroundComponent.OnGround);
        }
    }
}