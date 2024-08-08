namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class S2C_UnitRemoveCastHandler : MessageHandler<Scene, S2C_UnitRemoveCast>
    {
        protected override async ETTask Run(Scene entity, S2C_UnitRemoveCast message)
        {
            Room room = entity.GetComponent<Room>();
            LSWorld world = room.AuthorityWorld;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            LSUnit player = unitComponent.GetChild<LSUnit>(message.UnitId);
            LSUnit castUnit = unitComponent.GetChild<LSUnit>(message.CastUnitId);
            CastComponent castComponent = player.GetComponent<CastComponent>();
            Cast cast = (Cast)castUnit.Owner;
            castComponent.Remove(cast);
            await ETTask.CompletedTask;
        }
    }
}