using System;
using System.Collections.Generic;
using BulletSharp;

namespace ET
{
    public class CollisionCallbackAttribute : BaseAttribute
    {
        
    }
    
    [CollisionCallback]
    public abstract class ACollisionCallback : HandlerObject
    {
        public abstract void CollisionCallbackEnter(CollisionObject self, CollisionObject other);
        public abstract void CollisionCallbackStay(CollisionObject self, CollisionObject other);
        public abstract void CollisionCallbackExit(CollisionObject self, CollisionObject other);
    }
}