namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class S2C_UnitChangePositionHandler : MessageHandler<Scene, S2C_UnitChangePosition>
    {
        protected override async ETTask Run(Scene entity, S2C_UnitChangePosition message)
        {
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
            // unit位置改变会触发UnitChangePosition, 这里就不需要做其他的了
            // message.OldPos可能会在AOI用到
            
            await ETTask.CompletedTask;
        }
    }
}