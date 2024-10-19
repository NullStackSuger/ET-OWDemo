namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    [FriendOf(typeof(Room))]
    public class InputFinish_BroadCastUnPredictionMessage : AEvent<Scene, InputFinish>
    {
        protected override async ETTask Run(Scene scene, InputFinish a)
        {
            Room room = scene.GetComponent<Room>();
            UnPredictionMessage unPredictionMessage = UnPredictionMessage.Create();
            unPredictionMessage.Frame = room.PredictionFrame + 2;
            unPredictionMessage.PlayerId = room.PlayerId;
            unPredictionMessage.Look = room.Input.Look;
            scene.GetComponent<ClientSenderComponent>().Send(unPredictionMessage);
            await ETTask.CompletedTask;
        }
    }
}