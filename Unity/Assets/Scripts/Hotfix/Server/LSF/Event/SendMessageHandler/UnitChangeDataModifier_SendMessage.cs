namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    public class UnitChangeDataModifier_SendMessage : AEvent<LSWorld, DataModifierChange>
    {
        protected override async ETTask Run(LSWorld world, DataModifierChange a)
        {
            Room room = world.GetParent<Room>();
            if (room.AuthorityWorld == null) return;
            
            S2C_UnitChangeDataModifier message = S2C_UnitChangeDataModifier.Create();
            message.PlayerId = a.Unit.Id;
            message.DataModifierType = a.DataModifierType;
            message.Value = a.New;
        
            room.BroadCast(message);
            
            await ETTask.CompletedTask;
        }
    }
}