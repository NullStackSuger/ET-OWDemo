namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class UnitChangeDataModifier_ReceiveMessage : MessageHandler<Scene, S2C_UnitChangeDataModifier>
    {
        protected override async ETTask Run(Scene scene, S2C_UnitChangeDataModifier message)
        {
            if (!DataModifierType.Contains(message.DataModifierType)) return;
            
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
            
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            dataModifierComponent[message.DataModifierType] = message.Value;
            
            if (room.AuthorityWorld == null) return;
            unitComponent = room.AuthorityWorld.GetComponent<LSUnitComponent>();
            unit = unitComponent.GetChild<LSUnit>(message.PlayerId);
            if (unit == null)
            {
                return;
            }
            dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            dataModifierComponent[message.DataModifierType] = message.Value;
            
            await ETTask.CompletedTask;
        }
    }
}