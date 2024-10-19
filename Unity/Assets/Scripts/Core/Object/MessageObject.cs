using System;
using System.ComponentModel;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [DisableNew]
    [MemoryPackable(GenerateType.NoGenerate)]
    public abstract partial class MessageObject: ProtoObject, IMessage, IDisposable, IPool
    {
        public virtual void Dispose()
        {
        }

        [BsonIgnore]
        [MemoryPackIgnore]
        public bool IsFromPool { get; set; }
    }
}