using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class LSSceneChange_CreatRoom: AEvent<Scene, LSFSceneChange>
    {
        protected override async ETTask Run(Scene clientScene, LSFSceneChange args)
        {
            await UIHelper.Create(args.Room, UIType.LSF_UIRoom, UILayer.Low);
        }
    }
}