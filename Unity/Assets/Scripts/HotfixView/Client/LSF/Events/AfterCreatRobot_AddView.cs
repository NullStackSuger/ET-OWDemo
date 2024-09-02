namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class AfterCreatRobot_AddView : AEvent<Scene, AfterCreatRobot>
    {
        protected override async ETTask Run(Scene scene, AfterCreatRobot a)
        {
            scene.AddComponent<UIGlobalComponent>();
            scene.AddComponent<UIComponent>();
            scene.AddComponent<ResourcesLoaderComponent>();
            await ETTask.CompletedTask;
        }
    }
}