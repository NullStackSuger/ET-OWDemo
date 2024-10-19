namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class UnitChangeHeadRotation_ReceiveMessage : MessageHandler<Scene, S2C_UnitChangeHeadRotation>
    {
        protected override async ETTask Run(Scene scene, S2C_UnitChangeHeadRotation message)
        {
            Room room = scene.GetComponent<Room>();
            room.Record(room.AuthorityFrame, $"{message.UnitId}_{message.GetType()}", message);
            
            if (room.PredictionWorld == null) return;
            LSUnitComponent unitComponent = room.PredictionWorld.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(message.UnitId);
            if (unit == null)
            {
                return;
            }
            unit.HeadRotation = message.HeadRotation;
            
            if (room.AuthorityWorld == null) return;
            unitComponent = room.AuthorityWorld.GetComponent<LSUnitComponent>();
            unit = unitComponent.GetChild<LSUnit>(message.UnitId);
            if (unit == null)
            {
                return;
            }
            unit.HeadRotation = message.HeadRotation;
            
            await ETTask.CompletedTask;
        }
    }
}