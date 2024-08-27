using BulletSharp;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class B3CollisionComponent : LSEntity, IAwake<int>, IDestroy, ILSUpdate
    {
        public CollisionObject Collision { get; set; }

        public TSVector Offset { get; set; }
    }
}