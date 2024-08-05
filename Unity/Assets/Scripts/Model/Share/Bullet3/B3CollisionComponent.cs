using System.Collections.Generic;
using BulletSharp;
using BulletSharp.Math;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class B3CollisionComponent : LSEntity, IAwake<RigidBody>, IAwake<RigidBodyConstructionInfo, ACollisionCallback>, IDestroy, ILSUpdate
    {
        public RigidBody Body;
    }
}