using BulletSharp;
using BulletSharp.Math;

namespace ET
{
    [EntitySystemOf(typeof(B3WorldComponent))]
    [LSEntitySystemOf(typeof(B3WorldComponent))]
    [FriendOf(typeof(B3WorldComponent))]
    public static partial class B3WorldComponentSystem
    {
        [EntitySystem]
        private static void Awake(this B3WorldComponent self)
        {
            var CollisionConf = new DefaultCollisionConfiguration();
            var Dispatcher = new CollisionDispatcher(CollisionConf);
            var BroadPhase = new DbvtBroadphase();
            self.World = new DiscreteDynamicsWorld(Dispatcher, BroadPhase, null, CollisionConf);
            self.World.Gravity = new Vector3(0, 0, 0);
        }

        [EntitySystem]
        private static void Destroy(this B3WorldComponent self)
        {
            self.World.Dispose();
            self.World = null;
        }

        [LSEntitySystem]
        private static void LSUpdate(this B3WorldComponent self)
        {
            LSWorld world = self.GetParent<LSWorld>();
            Room room = world.GetParent<Room>();
            
            self.World.StepSimulation(1.0f / room.FixedTimeCounter.Interval);
                    
            foreach (var pair in self.Callbacks)
            {
                self.World.ContactTest(pair.Key, pair.Value);
            }
        }
        
        public static RigidBody AddBody(this B3WorldComponent self, RigidBodyConstructionInfo info, ContactResultCallback callback = null)
        {
            RigidBody body = new RigidBody(info);
            self.World.AddRigidBody(body);
            if(callback != null) self.Callbacks.Add(body, callback);
            return body;
        }
    }
}