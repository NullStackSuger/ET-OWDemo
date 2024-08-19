namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class S2C_UnitChangeRotationHandler : MessageHandler<Scene, S2C_UnitChangeRotation>
    {
        protected override async ETTask Run(Scene entity, S2C_UnitChangeRotation message)
        {
            using var _ = message; // 方法结束时回收消息
            
            Room room = entity.GetComponent<Room>();
            LSWorld world = room.AuthorityWorld;
            if (world == null) return;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(message.UnitId);
            if (unit == null) return;
            unit.Rotation = message.NewRotation;
            
            await ETTask.CompletedTask;
        }
    }
}