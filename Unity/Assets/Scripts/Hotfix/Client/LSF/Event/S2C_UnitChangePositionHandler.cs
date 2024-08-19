namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class S2C_UnitChangePositionHandler : MessageHandler<Scene, S2C_UnitChangePosition>
    {
        protected override async ETTask Run(Scene entity, S2C_UnitChangePosition message)
        {
            using var _ = message; // 方法结束时回收消息
            
            Room room = entity.GetComponent<Room>();
            LSWorld world = room.AuthorityWorld;
            if (world == null)
            {
                Log.Warning($"缺少World, 第{room.AuthorityFrame}帧");
                return;
            }
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(message.UnitId);
            if (unit == null)
            {
                Log.Warning($"缺少Unit: {message.UnitId}, 第{room.AuthorityFrame}帧");
                return;
            }
            unit.Position = message.NewPosition;
            
            await ETTask.CompletedTask;
        }
    }
}