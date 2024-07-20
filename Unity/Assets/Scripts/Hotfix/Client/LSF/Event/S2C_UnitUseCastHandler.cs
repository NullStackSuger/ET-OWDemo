namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class S2C_UnitUseCastHandler : MessageHandler<Scene, S2C_UnitUseCast>
    {
        protected override async ETTask Run(Scene entity, S2C_UnitUseCast message)
        {
            Room room = entity.GetComponent<Room>();
            LSWorld world = room.AuthorityWorld;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(message.UnitId);
            CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(message.CastConfigId, message.CastUnitId);
            
            await ETTask.CompletedTask;
        }
    }
}