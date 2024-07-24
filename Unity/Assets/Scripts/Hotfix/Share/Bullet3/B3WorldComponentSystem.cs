using System.Collections.Generic;
using System.IO;
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

            LSWorld world = self.GetParent<LSWorld>();
            Room room = world.GetParent<Room>();
            self.CreatSceneRb(room.Name).Coroutine();
        }

        private static async ETTask CreatSceneRb(this B3WorldComponent self, string name)
        {
            string path = $"D:\\{name}.bytes";
            if (!File.Exists(path)) return;
            byte[] bytes = await File.ReadAllBytesAsync(path);
            List<MeshInfo> infos = MemoryPackHelper.Deserialize(typeof(List<MeshInfo>), bytes, 0, bytes.Length) as List<MeshInfo>;
            
            // TODO: 这里加载时间>200s RPC会断开, 需要处理
            foreach (MeshInfo info in infos)
            {
                self.AddBody(info.Points, info.Position, info.Mass);
                await ETTask.CompletedTask;
            }
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
            if (info.Mass == 0)
            {
                body.CollisionFlags |= CollisionFlags.KinematicObject;
                body.ActivationState = ActivationState.DisableDeactivation;
            }
            
            if(callback != null) self.Callbacks.Add(body, callback);
            return body;
        }

        /// <summary>
        /// 场景中物体会使用这个来创建碰撞
        /// </summary>
        public static RigidBody AddBody(this B3WorldComponent self, Vector3[] points, Vector3 position, float mass = 0)
        {
            ConvexHullShape shape = new(points);
            shape.InitializePolyhedralFeatures();
            DefaultMotionState motionState = new DefaultMotionState(Matrix.Translation(position));
            Vector3 localInertia = Vector3.Zero;
            if (mass != 0) shape.CalculateLocalInertia(mass, out localInertia);
            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(0, motionState, shape, localInertia);
            RigidBody body = new(rbInfo);
            if (mass == 0)
            {
                body.CollisionFlags |= CollisionFlags.KinematicObject;
                body.ActivationState = ActivationState.DisableDeactivation;
            }
            self.World.AddRigidBody(body);

            return body;
        }
    }
}