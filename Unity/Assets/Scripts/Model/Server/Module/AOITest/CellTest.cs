using System.Collections.Generic;

namespace ET.Server
{
    [ChildOf(typeof(AOIManagerComponent))]
    public class CellTest : Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 在这个Cell的单位
        /// </summary>
        public Dictionary<long, EntityRef<AOIEntityTest>> Units = new();
    }
}