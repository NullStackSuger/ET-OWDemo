namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class UnitUseBuff_ReceiveMessage : MessageHandler<Scene, S2C_UnitUseBuff>
    {
        protected override async ETTask Run(Scene scene, S2C_UnitUseBuff message)
        {
            Room room = scene.GetComponent<Room>();
            room.Record(room.AuthorityFrame, $"{message.OwnerId}_{message.GetType()}", message);
            
            if (room.PredictionWorld == null) return;
            LSUnitComponent unitComponent = room.PredictionWorld.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(message.OwnerId);
            if (unit == null)
            {
                Log.Error($"{message.OwnerId}的Unit在权威World不存在");
                return;
            }
            unit.GetComponent<BuffComponent>().Creat(message.ConfigId, message.BuffId);
            
            if (room.AuthorityWorld == null) return;
            unitComponent = scene.GetComponent<Room>().AuthorityWorld.GetComponent<LSUnitComponent>();
            unit = unitComponent.GetChild<LSUnit>(message.OwnerId);
            if (unit == null)
            {
                Log.Error($"{message.OwnerId}的Unit在预测World不存在");
                return;
            }
            unit.GetComponent<BuffComponent>().Creat(message.ConfigId, message.BuffId);
            
            await ETTask.CompletedTask;
        }
    }
}