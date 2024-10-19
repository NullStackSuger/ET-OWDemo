namespace ET.Client
{
    public static class LSFUnitHelper
    {
        public static LSUnit GetMyUnit(Scene root)
        {
            PlayerComponent playerComponent = root.GetComponent<PlayerComponent>();
            Room room = root.GetComponent<Room>();
            LSWorld world = room.PredictionWorld;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            LSUnit unit = unitComponent.GetChild<LSUnit>(playerComponent.MyId);
            return unit;
        }
        
        public static LSUnit Creat(this LSUnitComponent self, LockStepUnitInfo playerInfo, string tag)
        {
            LSUnit unit = self.Creat(playerInfo.PlayerId);
            unit.Position = playerInfo.Position;
            unit.Rotation = playerInfo.Rotation;
            unit.Tag = tag;
            unit.AddComponent<ActionComponent, int>(playerInfo.ActionGroup);
            unit.AddComponent<BuffComponent>();
            unit.AddComponent<CastComponent>();
            unit.AddComponent<LSFInputComponent>();
            unit.AddComponent<DataModifierComponent>();
            return unit;
        }
    }
}