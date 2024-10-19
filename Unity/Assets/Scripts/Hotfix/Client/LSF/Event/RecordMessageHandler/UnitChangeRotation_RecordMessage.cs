/*namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    [FriendOf(typeof(Room))]
    public class UnitChangeRotation_RecordMessage : AEvent<LSWorld, UnitChangeRotation>
    {
        protected override async ETTask Run(LSWorld world, UnitChangeRotation a)
        {
            S2C_UnitChangeRotation message = S2C_UnitChangeRotation.Create();
            message.UnitId = a.Unit.Id;
            message.Rotation = a.NewRotation;
            
            Room room = world.GetParent<Room>();
            room.DeltaEvents[message.UnitId, typeof(S2C_UnitChangeRotation)] = message;
            
            await ETTask.CompletedTask;
        }
    }
}*/