namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    [FriendOf(typeof(LSF_UIRoomComponent))]
    public class NumbericChange_Hp : AEvent<LSWorld, DataModifierChange>
    {
        protected override async ETTask Run(LSWorld world, DataModifierChange a)
        {
            if (a.DataModifierType != DataModifierType.Hp) return;

            UIComponent uiComponent = world.Root().GetComponent<UIComponent>();
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