namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class UnitChangeRotation_ReceiveMessage : MessageHandler<Scene, S2C_UnitChangeRotation>
    {
        protected override async ETTask Run(Scene scene, S2C_UnitChangeRotation message)
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
                return;
            }
            unit.Rotation = message.Rotation;
            
            if (room.AuthorityWorld == null) return;
            unitComponent = room.AuthorityWorld.GetComponent<LSUnitComponent>();
            unit = unitComponent.GetChild<LSUnit>(message.PlayerId);
            if (unit == null)
            {
                return;
            }
            unit.Rotation = message.Rotation;
            
            await ETTask.CompletedTask;
        }
    }
}