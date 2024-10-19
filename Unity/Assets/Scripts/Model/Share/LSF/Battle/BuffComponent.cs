using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BuffComponent : LSEntity, IAwake, ISerializeToEntity
    {
        public Dictionary<string, EntityRef<Buff>> Buffs = new();
    }
}