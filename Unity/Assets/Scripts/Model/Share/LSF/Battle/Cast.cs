
using MemoryPack;
using TrueSync;

namespace ET
{
    [ChildOf(typeof(CastComponent))]
    [MemoryPackable]
    public partial class Cast : LSEntity, IAwake<int>, IAwake<int, TSVector>, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }

        [MemoryPackAllowSerialize]
        public EntityRef<LSUnit> Unit;
    }
}