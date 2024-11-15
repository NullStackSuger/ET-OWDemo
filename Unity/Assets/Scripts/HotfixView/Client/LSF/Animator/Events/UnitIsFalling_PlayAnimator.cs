namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    public class UnitIsFalling_PlayAnimator : AEvent<LSWorld, UnitOnGround>
    {
        protected override async ETTask Run(LSWorld scene, UnitOnGround a)
        {
            LSFUnitView view = scene.GetParent<Room>().GetComponent<LSFUnitViewComponent>().GetChild<LSFUnitView>(a.Unit.Id);

            // 有可能第1帧Server的Update, View还没创建
            if (view == null)
            {
                return;
            }
            
            var animator = view.GetComponent<AnimancerComponent>();
            if (a.OnGround)
            {
                animator.Play(AnimatorState.Idle);
            }
            else
            {
                animator.Play(AnimatorState.Jump);   
            }
            
            await ETTask.CompletedTask;
        }
    }
}