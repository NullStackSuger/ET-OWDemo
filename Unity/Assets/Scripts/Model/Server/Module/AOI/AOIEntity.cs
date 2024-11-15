using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(LSUnit))]
    public class AOIEntity : LSEntity, IAwake<float, float, int>, IDestroy
    {
        public int Radius { get; set; }
        public Cell Cell { get; set; }
        public List<Cell> AOICells { get; set; } = new List<Cell>();
        
        public List<AOIEntity> BeSee { get; set; } = new List<AOIEntity>();
    }
}