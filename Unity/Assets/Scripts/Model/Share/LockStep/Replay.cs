using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [MemoryPackable]
    public partial class Replay: Object
    {
        /// <summary>
        /// 主玩家Id
        /// </summary>
        [MemoryPackOrder(1)]
        public long PlayerId;
        
        /// <summary>
        /// 所有玩家信息
        /// </summary>
        [MemoryPackOrder(2)]
        public List<LockStepUnitInfo> UnitInfos;
        
        /// <summary>
        /// 所有帧Delta
        /// </summary>
        [MemoryPackOrder(3)]
        public List<OneFrameDeltaEvents> DeltaEvents = new();
        
        /// <summary>
        /// 所有帧输入
        /// </summary>
        [MemoryPackOrder(4)]
        public List<OneFrameInputs> FrameInputs = new();
        
        /// <summary>
        /// 每分钟快照
        /// </summary>
        [MemoryPackOrder(5)]
        public List<byte[]> Snapshots = new();
    }
}