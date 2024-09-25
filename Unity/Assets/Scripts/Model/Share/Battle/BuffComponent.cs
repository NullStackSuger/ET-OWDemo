using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class BuffComponent : LSEntity, IAwake
    {
        public Dictionary<string, EntityRef<Buff>> Buffs = new();
    }
}