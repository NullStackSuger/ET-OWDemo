namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class UnitOnGround_ReceiveMessage : MessageHandler<Scene, S2C_UnitOnGround>
    {
        protected override async ETTask Run(Scene scene, S2C_UnitOnGround message)
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
            await EventSystem.Instance.PublishAsync(room.AuthorityWorld, new UnitOnGround() { Unit = unit, OnGround = message.OnGround });
            
            if (room.AuthorityWorld == null) return;
            unitComponent = room.AuthorityWorld.GetComponent<LSUnitComponent>();
            unit = unitComponent.GetChild<LSUnit>(message.PlayerId);
            if (unit == null)
            {
                Log.Error($"{message.PlayerId}的Unit在预测World不存在");
                return;
            }
            await EventSystem.Instance.PublishAsync(room.PredictionWorld, new UnitOnGround() { Unit = unit, OnGround = message.OnGround });
        }
    }
}