namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class LoginFinish_RemoveLogin: AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            await UIHelper.Remove(scene, UIType.LSF_UILogin);
        }
    }
}