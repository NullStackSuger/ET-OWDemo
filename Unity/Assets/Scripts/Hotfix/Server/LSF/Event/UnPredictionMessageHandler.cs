namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    public class UnPredictionMessageHandler : MessageHandler<Scene, UnPredictionMessage>
    {
        protected override async ETTask Run(Scene entity, UnPredictionMessage message)
        {
            Room room = entity.GetComponent<Room>();
            if (message.Frame <= room.AuthorityFrame) return;
            if (!room.PlayerIds.Contains(message.PlayerId)) return;
            room.BroadCast(message);
            await ETTask.CompletedTask;
        }
    }
}