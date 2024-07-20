namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class LoginFinish_CreatLobby : AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish a)
        {
            await UIHelper.Create(scene, UIType.LSF_UILobby, UILayer.Mid);
        }
    }
}