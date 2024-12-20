using MemoryPack;

namespace ET
{
    [MemoryPackUnionFormatter(typeof(MessageObject))]
    [MemoryPackUnion(0, typeof(S2C_UnitChangePosition))]
    [MemoryPackUnion(1, typeof(S2C_UnitChangeRotation))]
    [MemoryPackUnion(2, typeof(S2C_UnitChangeHeadRotation))]
    [MemoryPackUnion(3, typeof(S2C_UnitUseCast))]
    [MemoryPackUnion(4, typeof(S2C_UnitRemoveCast))]
    [MemoryPackUnion(5, typeof(S2C_UnitUseBuff))]
    [MemoryPackUnion(6, typeof(S2C_UnitRemoveBuff))]
    [MemoryPackUnion(7, typeof(OneFrameDeltaEvents))]
    [MemoryPackUnion(8, typeof(S2C_UnitChangeDataModifier))]
    [MemoryPackUnion(9, typeof(S2C_UnitOnGround))]
    [EnableClass]
    public partial class MessageFormatter
    {

    }
}