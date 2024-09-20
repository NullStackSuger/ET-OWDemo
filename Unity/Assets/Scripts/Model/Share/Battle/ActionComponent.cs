using System.Collections.Generic;

namespace ET
{
    // Unit, Buff
    [ComponentOf]
    public class ActionComponent : LSEntity, IAwake<int>, IDestroy, ILSUpdate
    {
        public int Group { get; set; }

        public List<ActionConfig> Configs = new();

        public Dictionary<ActionConfig, bool> Actives = new();

        /// <summary>
        /// 注意 这个不能遍历, 因为在帧同步中, 每个客户端的字段顺序不确定
        /// </summary>
        public Dictionary<string, object> Args = new();
    }
}