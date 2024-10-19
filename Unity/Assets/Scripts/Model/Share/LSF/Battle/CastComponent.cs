using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CastComponent : LSEntity, IAwake, ISerializeToEntity
    {
        public List<EntityRef<Cast>> Casts = new();
    }
}