namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOf(typeof(Room))]
    public class UnitChangeRotation_SendMessage : AEvent<LSWorld, UnitChangeRotation>
    {
        protected override async ETTask Run(LSWorld world, UnitChangeRotation a)
        {
            Room room = world.GetParent<Room>();
            if (room.AuthorityWorld == null) return;
            
            S2C_UnitChangeRotation message = S2C_UnitChangeRotation.Create();
            message.UnitId = a.Unit.Id;
            message.Rotation = a.NewRotation;
            
            room.BroadCast(message);
            //room.DeltaEvents[message.UnitId, typeof(S2C_UnitChangeRotation)] = message;
            
            await ETTask.CompletedTask;
        }
    }
}