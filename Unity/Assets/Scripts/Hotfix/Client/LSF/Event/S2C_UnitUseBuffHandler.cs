namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class S2C_UnitUseBuffHandler : MessageHandler<Scene, S2C_UnitUseBuff>
    {
        protected override async ETTask Run(Scene entity, S2C_UnitUseBuff message)
        {
            Room room = entity.GetComponent<Room>();
            LSWorld world = room.AuthorityWorld;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(message.UnitId);
            BuffComponent buffComponent = unit.GetComponent<BuffComponent>();
            buffComponent.Creat(message.BuffConfigId);

            await ETTask.CompletedTask;
        }
    }
}