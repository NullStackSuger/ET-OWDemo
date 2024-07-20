using BulletSharp;
using BulletSharp.Math;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class B3CollisionComponent : LSEntity, IAwake<RigidBody>, IAwake<RigidBodyConstructionInfo, ContactResultCallback>, IDestroy, ILSUpdate
    {
        public RigidBody Body;
    }
}