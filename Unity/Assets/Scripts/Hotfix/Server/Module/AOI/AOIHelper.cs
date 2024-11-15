using System.Collections.Generic;

namespace ET.Server
{
    public static partial class AOIHelper
    {
        /// <summary>
        /// a关注到b创建的事件
        /// </summary>
        public static void OnEnter(AOIEntity a, AOIEntity b)
        {
            if (a == null || b == null) return;
            if (a == b) return;
            
            if (!b.BeSee.Contains(a)) b.BeSee.Add(a);
            EventSystem.Instance.Publish(a.Root(), new UnitEnterSightRange() { A = a, B = b });
        }
        
        /// <summary>
        /// a关注到b退出的事件
        /// </summary>
        public static void OnExit(AOIEntity a, AOIEntity b)
        {
            if (a == null || b == null) return;
            if (a == b) return;
            
            EventSystem.Instance.Publish(a.Root(), new UnitLeaveSightRange() { A = a, B = b });
            if (b.BeSee.Contains(a)) b.BeSee.Remove(a);
        }

        public static List<AOIEntity> GetAOI(this AOIEntity self)
        {
            return self.BeSee;
        }
    }
}