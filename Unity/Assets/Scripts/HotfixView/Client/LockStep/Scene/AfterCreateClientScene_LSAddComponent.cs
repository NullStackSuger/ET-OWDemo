namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class AfterCreateClientScene_LSAddComponent: AEvent<Scene, AfterCreateClientScene>
    {
        protected override async ETTask Run(Scene scene, AfterCreateClientScene args)
        {
            scene.AddComponent<UIComponent>();
            scene.AddComponent<ResourcesLoaderComponent>();
            await ETTask.CompletedTask;
        }
    }
}