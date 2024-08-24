using System.Collections.Generic;
using BulletSharp;
using BulletSharp.Math;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class B3CollisionComponent : LSEntity, IAwake<int>, IAwake<GhostObject>, IDestroy, ILSUpdate
    {
        public CollisionObject Collision;
    }
}