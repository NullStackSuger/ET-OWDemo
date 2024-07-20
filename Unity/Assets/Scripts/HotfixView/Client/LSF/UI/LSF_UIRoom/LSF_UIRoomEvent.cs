using System;
using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.LSF_UIRoom)]
    public class LSF_UIRoomEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
            string assetsName = $"Assets/Bundles/UI/LockStepFrame/{UIType.LSF_UIRoom}.prefab";
            GameObject bundleGameObject = await uiComponent.GetParent<Room>().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, uiComponent.UIGlobalComponent.GetLayer((int)uiLayer));
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.LSF_UIRoom, gameObject);
            ui.AddComponent<LSF_UIRoomComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
        }
    }
}