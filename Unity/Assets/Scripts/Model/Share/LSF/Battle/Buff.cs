using MemoryPack;

namespace ET
{
    [ChildOf(typeof(BuffComponent))]
    [MemoryPackable]
    public partial class Buff : LSEntity, IAwake<int>, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }
    }
}