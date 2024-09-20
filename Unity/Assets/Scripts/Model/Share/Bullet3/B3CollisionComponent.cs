using BulletSharp;
using BulletSharp.Math;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class B3CollisionComponent : LSEntity, IAwake<int>, IDestroy, ILSUpdate
    {
        public CollisionObject Collision { get; set; }

        public Vector3 Offset { get; set; }

        public static implicit operator CollisionObject(B3CollisionComponent collisionComponent)
        {
            return collisionComponent.Collision;
        }
    }
}