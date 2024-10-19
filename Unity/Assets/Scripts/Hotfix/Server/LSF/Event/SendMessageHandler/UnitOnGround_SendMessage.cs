namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    public class UnitOnGround_SendMessage : AEvent<LSWorld, UnitOnGround>
    {
        protected override async ETTask Run(LSWorld world, UnitOnGround a)
        {
            Room room = world.GetParent<Room>();
            if (room.AuthorityWorld == null) return;
            
            S2C_UnitOnGround message = S2C_UnitOnGround.Create();
            message.UnitId = a.Unit.Id;
            message.OnGround = a.OnGround;
            
            room.BroadCast(message);
            
            await ETTask.CompletedTask;
        }
    }
}