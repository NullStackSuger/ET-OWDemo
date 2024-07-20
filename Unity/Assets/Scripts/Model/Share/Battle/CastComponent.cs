using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class CastComponent : LSEntity, IAwake
    {
        public List<EntityRef<Cast>> Casts = new();
    }
}