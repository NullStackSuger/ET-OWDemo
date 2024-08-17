namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOfAttribute(typeof(ET.Buff))]
    public class UnitRemoveBuff_SendMessage : AEvent<LSWorld, UnitRemoveBuff>
    {
        protected override async ETTask Run(LSWorld world, UnitRemoveBuff a)
        {
            Room room = world.GetParent<Room>();
            
            var message = S2C_UnitRemoveBuff.Create(); 
            message.UnitId = a.Unit.Id;
            message.BuffId = a.Buff.Id;
            room.BroadCast(message);
            
            await ETTask.CompletedTask;
        }
    }
}