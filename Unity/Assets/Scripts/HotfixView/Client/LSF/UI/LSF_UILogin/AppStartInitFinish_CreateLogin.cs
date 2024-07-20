namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class AppStartInitFinish_CreateLogin: AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            await UIHelper.Create(root, UIType.LSF_UILogin, UILayer.Mid);
        }
    }
}