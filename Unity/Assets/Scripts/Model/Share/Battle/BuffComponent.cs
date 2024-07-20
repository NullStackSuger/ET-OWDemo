using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class BuffComponent : LSEntity, IAwake
    {
        public List<EntityRef<Buff>> Buffs = new();
    }
}