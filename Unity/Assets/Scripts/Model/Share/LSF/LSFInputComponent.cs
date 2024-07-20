using MemoryPack;

namespace ET
{
    // 不应该只存在于客户端, 服务端也需要这个来记录输入
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class LSFInputComponent: LSEntity, ILSUpdate, IAwake, ISerializeToEntity
    {
        public LSInput Input { get; set; }
        
        public int PressCastFrame { get; set; }
    }
}