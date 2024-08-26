using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 可以挂载在所有需要计时的组件上
    /// </summary>
    public class CDComponent : LSEntity, IAwake<long>, ILSUpdate, IDestroy
    {
        public long LastRecordTime;
        public long CD;
    }
}