using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [MemoryPackable]
    public partial class Replay: Object
    {
        /// <summary>
        /// 初始化信息
        /// </summary>
        [MemoryPackOrder(1)]
        public List<LockStepUnitInfo> UnitInfos;
        
        /// <summary>
        /// 所有帧输入
        /// </summary>
        [MemoryPackOrder(2)]
        public List<OneFrameDeltaEvents> DeltaEvents = new();
        
        /// <summary>
        /// 每分钟快照
        /// </summary>
        [MemoryPackOrder(3)]
        public List<byte[]> Snapshots = new();
    }
}