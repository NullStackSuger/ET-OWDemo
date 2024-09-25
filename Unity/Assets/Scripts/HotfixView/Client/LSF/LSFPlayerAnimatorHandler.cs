namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    [FriendOf(typeof(LSFAnimatorComponent))]
    public class LSFPlayerAnimatorHandler : AEvent<LSWorld, LSFPlayAnimator>
    {
        protected override async ETTask Run(LSWorld world, LSFPlayAnimator a)
        {
            Room room = world.GetParent<Room>();
            LSFUnitViewComponent unitViewComponent = room.GetComponent<LSFUnitViewComponent>();
            LSFUnitView unitView = unitViewComponent.GetChild<LSFUnitView>(a.UnitId);
            if (unitView == null)
            {
                Log.Error($"{a.UnitId}的UnitView不存在");
                return;
            }
            LSFAnimatorComponent animatorComponent = unitView.GetComponent<LSFAnimatorComponent>();
            if (animatorComponent == null)
            {
                Log.Error($"{a.UnitId}的Animator组件不存在");
                return;
            }

            animatorComponent.Animator.Play(a.AnimatorName);

            await ETTask.CompletedTask;
        }
    }
}