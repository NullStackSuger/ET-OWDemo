namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    public class UnitChangeMoveSpeed_PlayAnimator : AEvent<LSWorld, UnitChangeMoveSpeed>
    {
        protected override async ETTask Run(LSWorld scene, UnitChangeMoveSpeed a)
        {
            LSFUnitView view = scene.GetParent<Room>().GetComponent<LSFUnitViewComponent>().GetChild<LSFUnitView>(a.Unit.Id);

            // 有可能第1帧Server的Update, View还没创建
            if (view == null)
            {
                return;
            }
            
            view.GetComponent<LSFAnimatorComponent>().SetFloat("Speed", a.Speed);
            
            await ETTask.CompletedTask;
        }
    }
}