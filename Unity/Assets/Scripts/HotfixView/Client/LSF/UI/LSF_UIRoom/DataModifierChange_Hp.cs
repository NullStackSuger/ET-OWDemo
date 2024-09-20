namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    [FriendOf(typeof(LSF_UIRoomComponent))]
    public class DataModifierChange_Hp : AEvent<Scene, DataModifierChange>
    {
        protected override async ETTask Run(Scene scene, DataModifierChange a)
        {
            if (a.DataModifierType != DataModifierType.Hp) return;

            UIComponent uiComponent = scene.GetComponent<UIComponent>();
            UI ui = uiComponent.Get(UIType.LSF_UIRoom);
            LSF_UIRoomComponent roomComponent = ui.GetComponent<LSF_UIRoomComponent>();
            roomComponent.CurrentHp.text = a.New.ToString();
            if (roomComponent.MaxHp.text == "")
            {
                roomComponent.MaxHp.text = a.New.ToString();
            }

            await ETTask.CompletedTask;
        }
    }
}