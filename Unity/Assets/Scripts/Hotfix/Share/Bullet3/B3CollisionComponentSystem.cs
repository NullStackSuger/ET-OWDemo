using System.Collections.Generic;
using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(B3CollisionComponent))]
    [LSEntitySystemOf(typeof(B3CollisionComponent))]
    [FriendOf(typeof(B3CollisionComponent))]
    [FriendOf(typeof(B3WorldComponent))]
    public static partial class B3CollisionComponentSystem
    {
        [EntitySystem]
        private static void Awake(this B3CollisionComponent self, RigidBody body)
        {
            self.Body = body;
            body.UserObject = self;
        }

        [EntitySystem]
        private static void Awake(this B3CollisionComponent self, RigidBodyConstructionInfo info, ACollisionCallback callback = null)
        {
            RigidBody body = new RigidBody(info);
            //GhostObject body = new GhostObject();
            if (info.Mass == 0)
            {
                body.CollisionFlags |= CollisionFlags.KinematicObject;
                body.ActivationState = ActivationState.DisableDeactivation;
            }
            self.Body = body;
            body.UserObject = self;
         
            LSWorld world = self.IScene as LSWorld;
            B3WorldComponent worldComponent = world.GetComponent<B3WorldComponent>();
            worldComponent.NotifyToAdd(body, callback);

            LSUnit unit = self.GetParent<LSUnit>();
            self.Body.WorldTransform += Matrix.Translation((float)unit.Position.x, (float)unit.Position.y, (float)unit.Position.z);
            //self.Body.Orientation += new Quaternion((float)unit.Rotation.x, (float)unit.Rotation.y, (float)unit.Rotation.z, (float)unit.Rotation.w);
        }
        
        [EntitySystem]
        private static void Destroy(this B3CollisionComponent self)
        {
            LSWorld world = self.IScene as LSWorld;
            B3WorldComponent worldComponent = world.GetComponent<B3WorldComponent>();
            worldComponent.NotifyToRemove(self.Body);
            
            self.Body = null;
        }

        [LSEntitySystem]
        private static void LSUpdate(this B3CollisionComponent self)
        {
            self.Body.GetWorldTransform(out Matrix transform);
            LSUnit unit = self.GetParent<LSUnit>();

            unit.Position = new TSVector(transform.Origin.X, transform.Origin.Y, transform.Origin.Z);
            unit.Rotation = new TSQuaternion(transform.Orientation.X, transform.Orientation.Y, transform.Orientation.Z, transform.Orientation.W);
        }
    }
}