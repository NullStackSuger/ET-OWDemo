namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class LSFSceneInit_RemoveLobby : AEvent<Scene, LSFSceneInit>
    {
        protected override async ETTask Run(Scene scene, LSFSceneInit a)
        {
            await UIHelper.Remove(scene, UIType.LSF_UILobby);
        }
    }
}