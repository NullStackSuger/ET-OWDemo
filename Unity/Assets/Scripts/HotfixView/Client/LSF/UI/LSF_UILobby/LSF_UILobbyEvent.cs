using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.LSF_UILobby)]
    public class LSF_UILobbyEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
            string assetsName = $"Assets/Bundles/UI/LockStepFrame/{UIType.LSF_UILobby}.prefab";
            GameObject bundleGameObject = await uiComponent.Scene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject obj = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.LSF_UILobby, obj);
            ui.AddComponent<LSF_UILobbyComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            
        }
    }
}