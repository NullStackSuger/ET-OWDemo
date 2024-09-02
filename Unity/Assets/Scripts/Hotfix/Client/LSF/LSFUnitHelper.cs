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
    }
}