namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class UnitRemoveCast_ReceiveMessage : MessageHandler<Scene, S2C_UnitRemoveCast>
    {
        protected override async ETTask Run(Scene scene, S2C_UnitRemoveCast message)
        {
            Room room = scene.GetComponent<Room>();

            if (!room.IsReplay)
            {
                room.Record(room.AuthorityFrame, message.ToString(), message);
            }

            if (room.PredictionWorld == null) return;
            LSUnitComponent unitComponent = room.PredictionWorld.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(message.PlayerId);
            if (unit == null)
            {
                Log.Error($"{message.PlayerId}的Unit在权威World不存在");
                return;
            }
            unit.GetComponent<CastComponent>().Remove(message.CastId);
            
            if (room.AuthorityWorld == null) return;
            unitComponent = scene.GetComponent<Room>().AuthorityWorld.GetComponent<LSUnitComponent>();
            unit = unitComponent.GetChild<LSUnit>(message.PlayerId);
            if (unit == null)
            {
                Log.Error($"{message.PlayerId}的Unit在预测World不存在");
                return;
            }
            unit.GetComponent<CastComponent>().Remove(message.CastId);
            
            await ETTask.CompletedTask;
        }
    }
}