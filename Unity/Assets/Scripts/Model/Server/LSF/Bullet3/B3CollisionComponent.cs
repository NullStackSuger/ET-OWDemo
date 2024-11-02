using System.Collections.Generic;
using BulletSharp;
using BulletSharp.Math;

namespace ET.Server
{
    [ComponentOf(typeof(LSUnit))]
    public class B3CollisionComponent : LSEntity, IAwake<int>, IDestroy, ILSUpdate//, ISerializeToEntity
    {
        //[MemoryPackAllowSerialize]
        public CollisionObject Collision { get; set; }

        public Vector3 Offset { get; set; }

        public static implicit operator CollisionObject(B3CollisionComponent collisionComponent)
        {
            return collisionComponent.Collision;
        }
    }
}