namespace ET.Client
{
    public static class UIHelper
    {
        [EnableAccessEntiyChild]
        public static async ETTask<UI> Create(Entity scene, string uiType, UILayer uiLayer)
        {
            UIComponent uiComponent = scene.GetComponent<UIComponent>();
            if (uiComponent == null)
            {
                Log.Warning($"UI is null: {scene.IScene.SceneType} | {scene.Id} | {scene.InstanceId}");
                return null;
            }
            return await scene.GetComponent<UIComponent>().Create(uiType, uiLayer);
        }
        
        [EnableAccessEntiyChild]
        public static async ETTask Remove(Entity scene, string uiType)
        {
            /*UIComponent uiComponent = scene.GetComponent<UIComponent>();
            if (uiComponent == null) return;*/
            scene.GetComponent<UIComponent>().Remove(uiType);
            await ETTask.CompletedTask;
        }
    }
}