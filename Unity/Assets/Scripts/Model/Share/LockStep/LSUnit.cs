using System;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;
using Unity.Mathematics;

namespace ET
{
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

        [MemoryPackIgnore]
        [BsonIgnore]
        public TSVector Forward
        {
            get => this.Rotation * TSVector.forward;
            set => this.Rotation = TSQuaternion.LookRotation(value, TSVector.up);
        }

        private TSQuaternion rotation;
        public TSQuaternion Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                TSQuaternion oldRot = this.rotation;
                this.rotation = value;
                if (this.IScene == null) return;
                EventSystem.Instance.Publish(this.IScene as LSWorld, new UnitChangeRotation() { Unit = this, OldRotation = oldRot, NewRotation = value });
            }
        }

        //TODO 这里不确定对不对
        [MemoryPackIgnore]
        [BsonIgnore]
        public EntityRef<Entity> Owner { get; set; }
    }
}