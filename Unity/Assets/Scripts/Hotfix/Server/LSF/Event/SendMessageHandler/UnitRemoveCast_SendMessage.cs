namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOf(typeof(Room))]
    public class UnitRemoveCast_SendMessage : AEvent<LSWorld, UnitRemoveCast>
    {
        protected override async ETTask Run(LSWorld world, UnitRemoveCast a)
        {
            Room room = world.GetParent<Room>();
            if (room.AuthorityWorld == null) return;
            
            S2C_UnitRemoveCast message = S2C_UnitRemoveCast.Create();
            message.PlayerId = a.Owner.Id;
            message.CastId = a.Cast.Id;
   
            room.BroadCast(message);

            await ETTask.CompletedTask;
        }
    }
}