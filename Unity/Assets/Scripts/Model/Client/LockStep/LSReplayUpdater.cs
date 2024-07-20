namespace ET.Client
{
    [ComponentOf(typeof(ET.Room))]
    public class LSReplayUpdater: Entity, IAwake, IUpdate
    {
        public int ReplaySpeed { get; set; } = 1;
    }
}