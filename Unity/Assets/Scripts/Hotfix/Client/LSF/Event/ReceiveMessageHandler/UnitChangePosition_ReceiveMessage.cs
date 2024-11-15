namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class UnitChangePosition_ReceiveMessage : MessageHandler<Scene, S2C_UnitChangePosition>
    {
        protected override async ETTask Run(Scene scene, S2C_UnitChangePosition message)
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
            unit.Position = message.Position;
            
            if (room.AuthorityWorld == null) return;
            unitComponent = room.AuthorityWorld.GetComponent<LSUnitComponent>();
            unit = unitComponent.GetChild<LSUnit>(message.PlayerId);
            if (unit == null)
            {
                return;
            }
            unit.Position = message.Position;
            
            await ETTask.CompletedTask;
        }
    }
}