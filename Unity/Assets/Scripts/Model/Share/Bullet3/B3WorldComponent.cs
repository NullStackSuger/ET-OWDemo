using System.Collections.Generic;
using BulletSharp;

namespace ET
{
    [ComponentOf(typeof(LSWorld))]
    public class B3WorldComponent : LSEntity, IAwake, ILSUpdate, IDestroy
    {
        public DynamicsWorld World;
        public Dictionary<CollisionObject, ContactResultCallback> Callbacks = new();
    }
}