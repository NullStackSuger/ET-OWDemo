using System;

namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOfAttribute(typeof(ET.Cast))]
    public class UnitUseCast_SendMessage : AEvent<LSWorld, UnitUseCast>
    {
        protected override async ETTask Run(LSWorld world, UnitUseCast a)
        {
            Room room = world.GetParent<Room>();

            var message = S2C_UnitUseCast.Create(); 
            message.UnitId = a.Unit.Id;
            message.CastConfigId = a.Cast.ConfigId;
            room.BroadCast(message);

            await ETTask.CompletedTask;
        }
    }
}