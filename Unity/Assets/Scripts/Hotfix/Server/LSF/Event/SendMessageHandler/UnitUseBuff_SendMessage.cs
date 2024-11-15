namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOf(typeof(Room))]
    public class UnitUseBuff_SendMessage : AEvent<LSWorld, UnitUseBuff>
    {
        protected override async ETTask Run(LSWorld world, UnitUseBuff a)
        {
            Room room = world.GetParent<Room>();
            if (room.AuthorityWorld == null) return;
            
            S2C_UnitUseBuff message = S2C_UnitUseBuff.Create();
            message.PlayerId = a.Owner.Id;
            message.BuffId = a.Buff.Id;
            message.ConfigId = a.Buff.ConfigId;
            
            room.BroadCast(message);
            
            await ETTask.CompletedTask;
        }
    }
}