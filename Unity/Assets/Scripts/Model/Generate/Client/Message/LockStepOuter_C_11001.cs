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
        public TrueSync.TSQuaternion Rotation { get; set; }

        [MemoryPackOrder(3)]
        public int ActionGroup { get; set; }

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

    // ------------------------------------------------------------------------------------------------
    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitChangePosition)]
    public partial class S2C_UnitChangePosition : MessageObject, IMessage
    {
        public static S2C_UnitChangePosition Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitChangePosition), isFromPool) as S2C_UnitChangePosition;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public TSVector OldPosition { get; set; }

        [MemoryPackOrder(2)]
        public TSVector NewPosition { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.OldPosition = default;
            this.NewPosition = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitChangeRotation)]
    public partial class S2C_UnitChangeRotation : MessageObject, IMessage
    {
        public static S2C_UnitChangeRotation Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitChangeRotation), isFromPool) as S2C_UnitChangeRotation;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public TSQuaternion OldRotation { get; set; }

        [MemoryPackOrder(2)]
        public TSQuaternion NewRotation { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.OldRotation = default;
            this.NewRotation = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitUseCast)]
    public partial class S2C_UnitUseCast : MessageObject, IMessage
    {
        public static S2C_UnitUseCast Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitUseCast), isFromPool) as S2C_UnitUseCast;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public int CastConfigId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.CastConfigId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitRemoveCast)]
    public partial class S2C_UnitRemoveCast : MessageObject, IMessage
    {
        public static S2C_UnitRemoveCast Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitRemoveCast), isFromPool) as S2C_UnitRemoveCast;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public long CastId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.CastId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitUseBuff)]
    public partial class S2C_UnitUseBuff : MessageObject, IMessage
    {
        public static S2C_UnitUseBuff Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitUseBuff), isFromPool) as S2C_UnitUseBuff;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public int BuffConfigId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.BuffConfigId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.S2C_UnitRemoveBuff)]
    public partial class S2C_UnitRemoveBuff : MessageObject, IMessage
    {
        public static S2C_UnitRemoveBuff Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(S2C_UnitRemoveBuff), isFromPool) as S2C_UnitRemoveBuff;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public long BuffId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.BuffId = default;

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
        public const ushort S2C_UnitChangePosition = 11014;
        public const ushort S2C_UnitChangeRotation = 11015;
        public const ushort S2C_UnitUseCast = 11016;
        public const ushort S2C_UnitRemoveCast = 11017;
        public const ushort S2C_UnitUseBuff = 11018;
        public const ushort S2C_UnitRemoveBuff = 11019;
    }
}