namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class S2C_UnitChangeRotationHandler : MessageHandler<Scene, S2C_UnitChangeRotation>
    {
        protected override async ETTask Run(Scene entity, S2C_UnitChangeRotation message)
        {
            Room room = entity.GetComponent<Room>();
            LSWorld world = room.AuthorityWorld;
            if (world == null) return;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(message.UnitId);
            if (unit == null) return;
            unit.Rotation = message.NewRotation;
            // unit方向改变会触发UnitChangeRotation, 这里就不需要做其他的了
            // message.OldRot可能会在AOI用到
            
            await ETTask.CompletedTask;
        }
    }
}