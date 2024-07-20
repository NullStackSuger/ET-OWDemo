namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    public class UnitChangeRotation_SendMessage : AEvent<LSWorld, UnitChangeRotation>
    {
        protected override async ETTask Run(LSWorld world, UnitChangeRotation a)
        {
            Room room = world.GetParent<Room>();
            
            var message = S2C_UnitChangeRotation.Create();
            message.UnitId = a.Unit.Id;
            message.OldRotation = a.OldRotation;
            message.NewRotation = a.NewRotation;
            room.BroadCast(message);
            
            await ETTask.CompletedTask;
        }
    }
}