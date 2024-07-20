using System.Collections.Generic;

namespace ET
{
    // Unit, Cast, Buff
    [ComponentOf]
    public class ActionComponent : LSEntity, IAwake<int>, IDestroy, ILSUpdate
    {
        public int Group { get; set; }

        public List<ActionConfig> Configs = new();
    }
}