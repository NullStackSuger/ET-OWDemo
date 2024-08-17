namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class S2C_UnitRemoveBuffHandler : MessageHandler<Scene, S2C_UnitRemoveBuff>
    {
        protected override async ETTask Run(Scene entity, S2C_UnitRemoveBuff message)
        {
            Room room = entity.GetComponent<Room>();
            LSWorld world = room.AuthorityWorld;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(message.UnitId);
            BuffComponent buffComponent = unit.GetComponent<BuffComponent>();
            buffComponent.Remove(message.BuffId);
            
            await ETTask.CompletedTask;
        }
    }
}