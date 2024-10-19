/*namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    [FriendOf(typeof(Room))]
    public class UnitChangeHeadRotation_RecordMessage : AEvent<LSWorld, UnitChangeHeadRotation>
    {
        protected override async ETTask Run(LSWorld world, UnitChangeHeadRotation a)
        {
            S2C_UnitChangeHeadRotation message = S2C_UnitChangeHeadRotation.Create();
            message.UnitId = a.Unit.Id;
            message.HeadRotation = a.NewRotation;
            
            Room room = world.GetParent<Room>();
            room.DeltaEvents[message.UnitId, typeof(S2C_UnitChangeHeadRotation)] = message;
            
            await ETTask.CompletedTask;
        }
    }
}*/