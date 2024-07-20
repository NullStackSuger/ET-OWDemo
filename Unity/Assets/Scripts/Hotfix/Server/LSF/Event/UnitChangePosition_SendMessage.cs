namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    public class UnitChangePosition_SendMessage : AEvent<LSWorld, UnitChangePosition>
    {
        protected override async ETTask Run(LSWorld world, UnitChangePosition a)
        {
            Room room = world.GetParent<Room>();
            
            var message = S2C_UnitChangePosition.Create();
            message.UnitId = a.Unit.Id;
            message.OldPosition = a.OldPosition;
            message.NewPosition = a.NewPosition;
            room.BroadCast(message);
            
            
            await ETTask.CompletedTask;
        }
    }
}