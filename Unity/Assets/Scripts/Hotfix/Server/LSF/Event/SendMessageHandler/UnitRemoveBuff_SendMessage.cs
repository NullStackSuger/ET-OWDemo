namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOf(typeof(Room))]
    public class UnitRemoveBuff_SendMessage : AEvent<LSWorld, UnitRemoveBuff>
    {
        protected override async ETTask Run(LSWorld world, UnitRemoveBuff a)
        {
            Room room = world.GetParent<Room>();
            if (room.AuthorityWorld == null) return;
            
            S2C_UnitRemoveBuff message = S2C_UnitRemoveBuff.Create();
            message.PlayerId = a.Owner.Id;
            message.BuffId = a.Buff.Id;
            
            room.BroadCast(message);
            
            await ETTask.CompletedTask;
        }
    }
}