namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOfAttribute(typeof(ET.Buff))]
    public class UnitUseBuff_SendMessage : AEvent<LSWorld, UnitUseBuff>
    {
        protected override async ETTask Run(LSWorld world, UnitUseBuff a)
        {
            Room room = world.GetParent<Room>();
            
            var message = S2C_UnitUseBuff.Create(); 
            message.UnitId = a.Unit.Id;
            message.BuffConfigId = a.Buff.ConfigId;
            room.BroadCast(message);
            
            await ETTask.CompletedTask;
        }
    }
}