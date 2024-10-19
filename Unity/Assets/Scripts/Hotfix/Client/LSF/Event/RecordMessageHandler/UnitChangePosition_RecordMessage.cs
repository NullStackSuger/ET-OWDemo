/*namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    [FriendOf(typeof(Room))]
    public class UnitChangePosition_RecordMessage : AEvent<LSWorld, UnitChangePosition>
    {
        protected override async ETTask Run(LSWorld world, UnitChangePosition a)
        {
            S2C_UnitChangePosition message = S2C_UnitChangePosition.Create();
            message.UnitId = a.Unit.Id;
            message.Position = a.Position;

            Room room = world.GetParent<Room>();
            room.DeltaEvents[message.UnitId, typeof(S2C_UnitChangePosition)] = message;

            await ETTask.CompletedTask;
        }
    }
}*/