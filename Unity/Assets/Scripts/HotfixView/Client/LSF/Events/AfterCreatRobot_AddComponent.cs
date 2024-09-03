namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class AfterCreatRobot_AddComponent : AEvent<Scene, AfterCreatRobot>
    {
        protected override async ETTask Run(Scene scene, AfterCreatRobot a)
        {
            scene.AddComponent<GlobalComponent>();
            scene.AddComponent<UIGlobalComponent>();
            scene.AddComponent<UIComponent>();
            scene.AddComponent<ResourcesLoaderComponent>();
            await ETTask.CompletedTask;
        }
    }
}