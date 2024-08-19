namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOfAttribute(typeof(ET.Cast))]
    public class UnitRemoveCast_SendMessage : AEvent<LSWorld, UnitRemoveCast>
    {
        protected override async ETTask Run(LSWorld world, UnitRemoveCast a)
        {
            Room room = world.GetParent<Room>();

            var message = S2C_UnitRemoveCast.Create();
            LSUnit castUnit = a.Cast.Unit;
            if (castUnit == null) return;
            message.UnitId = castUnit.Id;
            message.CastId = a.Cast.Id;
            room.BroadCast(message);

            await ETTask.CompletedTask;
        }
    }
}