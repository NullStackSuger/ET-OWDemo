namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOf(typeof(Room))]
    public class UnitUseCast_SendMessage : AEvent<LSWorld, UnitUseCast>
    {
        protected override async ETTask Run(LSWorld world, UnitUseCast a)
        {
            Room room = world.GetParent<Room>();
            if (room.AuthorityWorld == null) return;
            
            S2C_UnitUseCast message = S2C_UnitUseCast.Create();
            message.OwnerId = a.Owner.Id;
            message.CastId = a.Cast.Id;
            message.ConfigId = a.Cast.ConfigId;
            
            room.BroadCast(message);

            await ETTask.CompletedTask;
        }
    }
}