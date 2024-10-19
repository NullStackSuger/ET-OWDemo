using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PortalComponent : LSEntity, IAwake<TSVector>, ISerializeToEntity
    {
        public TSVector Other;
    }
}