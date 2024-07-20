using TrueSync;

namespace ET.Client
{
    [ComponentOf(typeof(ET.Room))]
    public class LSClientUpdater: Entity, IAwake, IUpdate
    {
        public LSInput Input = new();
        
        public long MyId { get; set; }
    }
}