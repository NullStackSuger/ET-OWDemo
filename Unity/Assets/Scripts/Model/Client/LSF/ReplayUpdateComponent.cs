namespace ET.Client
{
    [ComponentOf(typeof(Room))]
    public class ReplayUpdateComponent : Entity, IAwake, IUpdate
    {
        public int ReplaySpeed = 1;
    }
}