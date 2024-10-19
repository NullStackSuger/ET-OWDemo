namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    [FriendOf(typeof(LSF_UIRoomComponent))]
    public class NumbericChange_BulletCount : AEvent<Scene, DataModifierChange>
    {
        protected override async ETTask Run(Scene scene, DataModifierChange a)
        {
            if (a.DataModifierType != DataModifierType.BulletCount) return;
            
            UIComponent uiComponent = scene.GetComponent<UIComponent>();
            UI ui = uiComponent.Get(UIType.LSF_UIRoom);
            LSF_UIRoomComponent roomComponent = ui.GetComponent<LSF_UIRoomComponent>();
            roomComponent.CurrentBulletCount.text = a.New.ToString();
            if (roomComponent.MaxBulletCount.text == "")
            {
                roomComponent.MaxBulletCount.text = a.New.ToString();
            }

            await ETTask.CompletedTask;
        }
    }
}