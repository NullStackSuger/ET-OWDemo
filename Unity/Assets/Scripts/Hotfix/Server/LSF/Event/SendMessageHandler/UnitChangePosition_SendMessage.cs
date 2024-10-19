namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    public class UnitChangePosition_SendMessage : AEvent<LSWorld, UnitChangePosition>
    {
        protected override async ETTask Run(LSWorld world, UnitChangePosition a)
        {
            Room room = world.GetParent<Room>();
            if (room.AuthorityWorld == null) return;
            
            S2C_UnitChangePosition message = S2C_UnitChangePosition.Create();
            message.UnitId = a.Unit.Id;
            message.Position = a.NewPosition;
        
            room.BroadCast(message);
            //room.DeltaEvents[message.UnitId, typeof(S2C_UnitChangePosition)] = message;
            
            await ETTask.CompletedTask;
        }
    }
}