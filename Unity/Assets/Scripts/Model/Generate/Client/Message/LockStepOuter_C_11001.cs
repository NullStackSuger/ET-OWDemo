using MemoryPack;
using TrueSync;
using System.Collections.Generic;

namespace ET
{
    [MemoryPackable]
    [Message(LockStepOuter.C2G_Match)]
    [ResponseType(nameof(G2C_Match))]
    public partial class C2G_Match : MessageObject, ISessionRequest
    {
        public static C2G_Match Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_Match), isFromPool) as C2G_Match;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.G2C_Match)]
    public partial class G2C_Match : MessageObject, ISessionResponse
    {
        public static G2C_Match Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Match), isFromPool) as G2C_Match;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 匹配成功，通知客户端切换场景
    /// </summary>
    [MemoryPackable]
    [Message(LockStepOuter.Match2G_NotifyMatchSuccess)]
    public partial class Match2G_NotifyMatchSuccess : MessageObject, IMessage
    {
        public static Match2G_NotifyMatchSuccess Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Match2G_NotifyMatchSuccess), isFromPool) as Match2G_NotifyMatchSuccess;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// 房间的ActorId
        /// </summary>
        [MemoryPackOrder(1)]
        public ActorId ActorId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ActorId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 客户端通知房间切换场景完成
    /// </summary>
    [MemoryPackable]
    [Message(LockStepOuter.C2Room_ChangeSceneFinish)]
    public partial class C2Room_ChangeSceneFinish : MessageObject, IRoomMessage
    {
        public static C2Room_ChangeSceneFinish Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_ChangeSceneFinish), isFromPool) as C2Room_ChangeSceneFinish;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.LockStepUnitInfo)]
    public partial class LockStepUnitInfo : MessageObject
    {
        public static LockStepUnitInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(LockStepUnitInfo), isFromPool) as LockStepUnitInfo;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public TrueSync.TSVector Position { get; set; }

        [MemoryPackOrder(2)]
        public FP Rotation { get; set; }

        [MemoryPackOrder(3)]
        public int ActionGroup { get; set; }

        [MemoryPackOrder(4)]
        public int RigidBodyId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.Position = default;
            this.Rotation = default;
            this.ActionGroup = default;
            this.RigidBodyId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 房间通知客户端进入战斗
    /// </summary>
    [MemoryPackable]
    [Message(LockStepOuter.Room2C_Start)]
    public partial class Room2C_Start : MessageObject, IMessage
    {
        public static Room2C_Start Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_Start), isFromPool) as Room2C_Start;
        }

        [MemoryPackOrder(0)]
        public long StartTime { get; set; }

        [MemoryPackOrder(1)]
        public List<LockStepUnitInfo> UnitInfo { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.StartTime = default;
            this.UnitInfo.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.FrameMessage)]
    public partial class FrameMessage : MessageObject, IMessage
    {
        public static FrameMessage Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(FrameMessage), isFromPool) as FrameMessage;
        }

        [MemoryPackOrder(0)]
        public int Frame { get; set; }

        [MemoryPackOrder(1)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(2)]
        public LSInput Input { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Frame = default;
            this.PlayerId = default;
            this.Input = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.OneFrameInputs)]
    public partial class OneFrameInputs : MessageObject, IMessage
    {
        public static OneFrameInputs Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(OneFrameInputs), isFromPool) as OneFrameInputs;
        }

        [MemoryPackOrder(0)]
        public int Frame { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
        [MemoryPackOrder(1)]
        public Dictionary<long, LSInput> Inputs { get; set; } = new();
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Frame = default;
            this.Inputs.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.Room2C_AdjustUpdateTime)]
    public partial class Room2C_AdjustUpdateTime : MessageObject, IMessage
    {
        public static Room2C_AdjustUpdateTime Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_AdjustUpdateTime), isFromPool) as Room2C_AdjustUpdateTime;
        }

        [MemoryPackOrder(0)]
        public int DiffTime { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.DiffTime = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.C2Room_CheckHash)]
    public partial class C2Room_CheckHash : MessageObject, IRoomMessage
    {
        public static C2Room_CheckHash Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_CheckHash), isFromPool) as C2Room_CheckHash;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public int Frame { get; set; }

        [MemoryPackOrder(2)]
        public long Hash { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.Frame = default;
            this.Hash = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.Room2C_CheckHashFail)]
    public partial class Room2C_CheckHashFail : MessageObject, IMessage
    {
        public static Room2C_CheckHashFail Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_CheckHashFail), isFromPool) as Room2C_CheckHashFail;
        }

        [MemoryPackOrder(0)]
        public int Frame { get; set; }

        [MemoryPackOrder(1)]
        public byte[] LSWorldBytes { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Frame = default;
            this.LSWorldBytes = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.G2C_Reconnect)]
    public partial class G2C_Reconnect : MessageObject, IMessage
    {
        public static G2C_Reconnect Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Reconnect), isFromPool) as G2C_Reconnect;
        }

        [MemoryPackOrder(0)]
        public long StartTime { get; set; }

        [MemoryPackOrder(1)]
        public List<LockStepUnitInfo> UnitInfos { get; set; } = new();

        [MemoryPackOrder(2)]
        public int Frame { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.StartTime = default;
            this.UnitInfos.Clear();
            this.Frame = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    // 类似Rot这种 连续的 不易预测的 需要预测的 消息使用
    [MemoryPackable]
    [Message(LockStepOuter.UnPredictionMessage)]
    public partial class UnPredictionMessage : MessageObject, IRoomMessage
    {
        public static UnPredictionMessage Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(UnPredictionMessage), isFromPool) as UnPredictionMessage;
        }

        [MemoryPackOrder(0)]
        public int Frame { get; set; }

        [MemoryPackOrder(1)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(2)]
        public TSVector2 Look { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Frame = default;
            this.PlayerId = default;
            this.Look = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.OneFrameDeltaEvents)]
    public partial class OneFrameDeltaEvents : MessageObject, IMessage
    {
        public static OneFrameDeltaEvents Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(OneFrameDeltaEvents), isFromPool) as OneFrameDeltaEvents;
        }

        [MemoryPackOrder(0)]
        public int Frame { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
        [MemoryPackOrder(1)]
        public Dictionary<string, MessageObject> Events { get; set; } = new();
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Frame = default;
            this.Events.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitChangePosition)]
    public partial class S2C_UnitChangePosition : MessageObject, IRoomMessage
    {
        public static S2C_UnitChangePosition Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitChangePosition), isFromPool) as S2C_UnitChangePosition;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public TSVector Position { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.Position = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitChangeRotation)]
    public partial class S2C_UnitChangeRotation : MessageObject, IRoomMessage
    {
        public static S2C_UnitChangeRotation Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitChangeRotation), isFromPool) as S2C_UnitChangeRotation;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public FP Rotation { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.Rotation = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitChangeHeadRotation)]
    public partial class S2C_UnitChangeHeadRotation : MessageObject, IRoomMessage
    {
        public static S2C_UnitChangeHeadRotation Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitChangeHeadRotation), isFromPool) as S2C_UnitChangeHeadRotation;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public FP HeadRotation { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.HeadRotation = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitUseCast)]
    public partial class S2C_UnitUseCast : MessageObject, IRoomMessage
    {
        public static S2C_UnitUseCast Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitUseCast), isFromPool) as S2C_UnitUseCast;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public long CastId { get; set; }

        [MemoryPackOrder(2)]
        public int ConfigId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.CastId = default;
            this.ConfigId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitRemoveCast)]
    public partial class S2C_UnitRemoveCast : MessageObject, IRoomMessage
    {
        public static S2C_UnitRemoveCast Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitRemoveCast), isFromPool) as S2C_UnitRemoveCast;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public long CastId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.CastId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitUseBuff)]
    public partial class S2C_UnitUseBuff : MessageObject, IRoomMessage
    {
        public static S2C_UnitUseBuff Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitUseBuff), isFromPool) as S2C_UnitUseBuff;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public long BuffId { get; set; }

        [MemoryPackOrder(2)]
        public int ConfigId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.BuffId = default;
            this.ConfigId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitRemoveBuff)]
    public partial class S2C_UnitRemoveBuff : MessageObject, IRoomMessage
    {
        public static S2C_UnitRemoveBuff Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitRemoveBuff), isFromPool) as S2C_UnitRemoveBuff;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public long BuffId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.BuffId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitChangeDataModifier)]
    public partial class S2C_UnitChangeDataModifier : MessageObject, IRoomMessage
    {
        public static S2C_UnitChangeDataModifier Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitChangeDataModifier), isFromPool) as S2C_UnitChangeDataModifier;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public int DataModifierType { get; set; }

        [MemoryPackOrder(2)]
        public long Value { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.DataModifierType = default;
            this.Value = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitOnGround)]
    public partial class S2C_UnitOnGround : MessageObject, IRoomMessage
    {
        public static S2C_UnitOnGround Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitOnGround), isFromPool) as S2C_UnitOnGround;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public bool OnGround { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.OnGround = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    public static class LockStepOuter
    {
        public const ushort C2G_Match = 11002;
        public const ushort G2C_Match = 11003;
        public const ushort Match2G_NotifyMatchSuccess = 11004;
        public const ushort C2Room_ChangeSceneFinish = 11005;
        public const ushort LockStepUnitInfo = 11006;
        public const ushort Room2C_Start = 11007;
        public const ushort FrameMessage = 11008;
        public const ushort OneFrameInputs = 11009;
        public const ushort Room2C_AdjustUpdateTime = 11010;
        public const ushort C2Room_CheckHash = 11011;
        public const ushort Room2C_CheckHashFail = 11012;
        public const ushort G2C_Reconnect = 11013;
        public const ushort UnPredictionMessage = 11014;
        public const ushort OneFrameDeltaEvents = 11015;
        public const ushort S2C_UnitChangePosition = 11016;
        public const ushort S2C_UnitChangeRotation = 11017;
        public const ushort S2C_UnitChangeHeadRotation = 11018;
        public const ushort S2C_UnitUseCast = 11019;
        public const ushort S2C_UnitRemoveCast = 11020;
        public const ushort S2C_UnitUseBuff = 11021;
        public const ushort S2C_UnitRemoveBuff = 11022;
        public const ushort S2C_UnitChangeDataModifier = 11023;
        public const ushort S2C_UnitOnGround = 11024;
    }
}