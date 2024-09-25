using System;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;
using Unity.Mathematics;

namespace ET
{
    public static class Tag
    {
        public const string None = "None";

        /// <summary>
        /// 这个不是用来赋值Tag的, 是用来和其他标签比较的
        /// </summary>
        public const string Player = "Player";
        public const string PlayerA = Player + "_A";
        public const string PlayerB = Player + "_B";
        
        /// <summary>
        /// 这个不是用来赋值Tag的, 是用来和其他标签比较的
        /// </summary>
        public const string Cast = "Cast";
        public const string CastA = Cast + "_A";
        public const string CastB = Cast + "_B";
        
        /// <summary>
        /// 这个不是用来赋值Tag的, 是用来和其他标签比较的
        /// </summary>
        public const string Shield = "Shield";
        public const string ShieldA = Shield + "_A";
        public const string ShieldB = Shield + "_B";

        /// <summary>
        /// 这个不是用来赋值Tag的, 是用来和其他标签比较的
        /// </summary>
        public const string CastPreview = "CastPreview";
        public const string CastPreviewA = CastPreview + "_A";
        public const string CastPreviewB = CastPreview + "_B";
        
        public const string Neutral = "Neutral"; // 中立

        public static bool IsFriend(string tagA, string tagB)
        {
            return tagA.Substring(tagA.Length - 1, 1) == tagB.Substring(tagB.Length - 1, 1);
        }
    }
    
    [ChildOf(typeof(LSUnitComponent))]
    [MemoryPackable]
    public partial class LSUnit: LSEntity, IAwake, IDestroy, ISerializeToEntity
    {
        private TSVector position;
        public TSVector Position
        {
            get
            {
                return this.position;
            }
            set
            {
                TSVector oldPos = this.position;
                this.position = value;
                if (this.IScene == null) return;
                EventSystem.Instance.Publish(this.IScene as LSWorld, new UnitChangePosition() { Unit = this, OldPosition = oldPos, NewPosition = value });
            }
        }

        /*[MemoryPackIgnore]
        [BsonIgnore]
        public TSVector Forward
        {
            get => this.Rotation * TSVector.forward;
            set => this.Rotation = TSQuaternion.LookRotation(value, TSVector.up);
        }*/

        private TSQuaternion rotation;
        public FP Rotation
        {
            get
            {
                return this.rotation.y;
            }
            set
            {
                FP oldRot = this.rotation.y;
                
                this.rotation.y = value;
                this.rotation.y %= 360;
                if (this.rotation.y < 0) this.rotation.y = 360 - this.rotation.y;
                
                if (this.IScene == null) return;
                EventSystem.Instance.Publish(this.IScene as LSWorld, new UnitChangeRotation() { Unit = this, OldRotation = oldRot, NewRotation = value });
            }
        }

        public FP HeadRotation
        {
            get
            {
                return this.rotation.x;
            }
            set
            {
                FP oldRot = this.rotation.x;
                this.rotation.x = TSMath.Clamp(value % 360, -90, 90);
                
                if (this.IScene == null) return;
                EventSystem.Instance.Publish(this.IScene as LSWorld, new UnitChangeHeadRotation() { Unit = this, OldRotation = oldRot, NewRotation = value });
            }
        }
        
        [MemoryPackIgnore]
        [BsonIgnore]
        public EntityRef<LSUnit> Owner { get; set; }
        
        public string Tag { get; set; } = ET.Tag.None;
    }
}