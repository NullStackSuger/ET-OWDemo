namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class S2C_UnitRemoveCastHandler : MessageHandler<Scene, S2C_UnitRemoveCast>
    {
        protected override async ETTask Run(Scene entity, S2C_UnitRemoveCast message)
        {
            using var _ = message; // 方法结束时回收消息
            
            Room room = entity.GetComponent<Room>();
            LSWorld world = room.AuthorityWorld;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            LSUnit castUnit = unitComponent.GetChild<LSUnit>(message.UnitId);
            LSUnit owner = castUnit.Owner;
            CastComponent castComponent = owner.GetComponent<CastComponent>();
            castComponent.Remove(message.CastId);
            
            await ETTask.CompletedTask;
        }
    }
}