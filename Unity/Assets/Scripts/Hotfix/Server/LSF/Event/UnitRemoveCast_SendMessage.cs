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
            message.UnitId = a.Unit.Id;
            LSUnit castUnit = a.Cast.Unit;
            message.CastUnitId = castUnit.Id;
            room.BroadCast(message);

            await ETTask.CompletedTask;
        }
    }
}