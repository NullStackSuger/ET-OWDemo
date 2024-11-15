namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOf(typeof(Room))]
    public class UnitChangeHeadRotation_SendMessage : AEvent<LSWorld, UnitChangeHeadRotation>
    {
        protected override async ETTask Run(LSWorld world, UnitChangeHeadRotation a)
        {
            Room room = world.GetParent<Room>();
            if (room.AuthorityWorld == null) return;
            
            S2C_UnitChangeHeadRotation message = S2C_UnitChangeHeadRotation.Create();
            message.PlayerId = a.Unit.Id;
            message.HeadRotation = a.NewRotation;
            
            room.BroadCast(message);
            //room.DeltaEvents[message.UnitId, typeof(S2C_UnitChangeHeadRotation)] = message;
            
            await ETTask.CompletedTask;
        }
    }
}