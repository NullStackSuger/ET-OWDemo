using System.Collections.Generic;
using System.IO;
using System.Linq;
using BulletSharp;
using BulletSharp.Math;

namespace ET
{
    [EntitySystemOf(typeof(B3WorldComponent))]
    [LSEntitySystemOf(typeof(B3WorldComponent))]
    [FriendOf(typeof(B3WorldComponent))]
    [FriendOf(typeof(B3CollisionComponent))]
    public static partial class B3WorldComponentSystem
    {
        [EntitySystem]
        private static void Awake(this B3WorldComponent self)
        {
            var CollisionConf = new DefaultCollisionConfiguration();
            var Dispatcher = new CollisionDispatcher(CollisionConf);
            var BroadPhase = new DbvtBroadphase();
            self.World = new DiscreteDynamicsWorld(Dispatcher, BroadPhase, null, CollisionConf);
            self.World.Broadphase.OverlappingPairCache.SetInternalGhostPairCallback(new GhostPairCallback());
            self.World.Gravity = new Vector3(0, 0, 0);

            LSWorld world = self.GetParent<LSWorld>();
            Room room = world.GetParent<Room>();
            CreatSceneRb(self, room.Name).Coroutine();

            async ETTask CreatSceneRb(B3WorldComponent self, string name)
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

            // 模拟物理
            self.World.StepSimulation(1.0f / room.FixedTimeCounter.Interval);

            // 处理碰撞
            self.LastCollisionInfos.Clear();
            self.LastCollisionInfos = self.NowCollisionInfos;
            self.NowCollisionInfos = new();
            
            // TODO 这里存在一个问题 不同客户端碰撞信息的index可能不同, 导致不同步
            Dispatcher dispatcher = self.World.Dispatcher;
            int count = dispatcher.NumManifolds;
            for (int i = 0; i < count; i++)
            {
                PersistentManifold contactManifold = dispatcher.GetManifoldByIndexInternal(i);
                if (contactManifold.NumContacts <= 0) continue;
                
                CollisionObject a = contactManifold.Body0;
                CollisionObject b = contactManifold.Body1;
                // 在Callbacks里存在说明是B3CollisionComponent并有回调
                if (self.Callbacks.ContainsKey(a))
                {
                    self.NowCollisionInfos.Add((a, b));
                }
                if (self.Callbacks.ContainsKey(b))
                {
                    self.NowCollisionInfos.Add((b, a));
                }
            }
            
            FixedHandler();
            
            // 更新碰撞列表
            while (self.WaitToAdds.TryDequeue(out var pair))
            {
                CollisionObject collisionObject = pair.Item1;
                ACollisionCallback callback = pair.Item2;
                self.World.AddCollisionObject(collisionObject);
                if (callback != null) self.Callbacks.Add(collisionObject, callback);
            }
            while (self.WaitToRemoves.TryDequeue(out CollisionObject collisionObject))
            {
                self.World.RemoveCollisionObject(collisionObject);
                if (self.Callbacks.ContainsKey(collisionObject)) self.Callbacks.Remove(collisionObject);
                collisionObject.Dispose();
            }
            
            void FixedHandler()
            {
                // 取Now差集是Enter
                var enter = self.NowCollisionInfos.Except(self.LastCollisionInfos);
                foreach (var pair in enter)
                {
                    CollisionObject a = pair.Item1;
                    CollisionObject b = pair.Item2;
                    
                    // collisionA collisionB 不为空 且 unitA unitB 的TeamTag不同
                    
                    B3CollisionComponent collisionA = a.UserObject as B3CollisionComponent;
                    B3CollisionComponent collisionB = b.UserObject as B3CollisionComponent;

                    if (collisionA == null || collisionB == null)
                    {
                        if (self.Callbacks.TryGetValue(a, out var callback))
                            callback.CollisionCallbackEnter(a, b);
                        
                        continue;
                    }
                    
                    TeamTag tagA = collisionA.GetParent<LSUnit>().Tag;
                    TeamTag tagB = collisionB.GetParent<LSUnit>().Tag;

                    if (tagA != tagB /*&& tagA != TeamTag.None*/)
                    {
                        if (self.Callbacks.TryGetValue(a, out var callback))
                            callback.CollisionCallbackEnter(a, b);
                        
                        continue;
                    }
                }
                // 取交集是Stay
                var stay = self.LastCollisionInfos.Intersect(self.NowCollisionInfos);
                foreach (var pair in stay)
                {
                    CollisionObject a = pair.Item1;
                    CollisionObject b = pair.Item2;
                    
                    // collisionA collisionB 不为空 且 unitA unitB 的TeamTag不同
                    
                    B3CollisionComponent collisionA = a.UserObject as B3CollisionComponent;
                    B3CollisionComponent collisionB = b.UserObject as B3CollisionComponent;

                    if (collisionA == null || collisionB == null)
                    {
                        if (self.Callbacks.TryGetValue(a, out var callback))
                            callback.CollisionCallbackStay(a, b);
                        
                        continue;
                    }
                    
                    TeamTag tagA = collisionA.GetParent<LSUnit>().Tag;
                    TeamTag tagB = collisionB.GetParent<LSUnit>().Tag;

                    if (tagA != tagB /*&& tagA != TeamTag.None*/)
                    {
                        if (self.Callbacks.TryGetValue(a, out var callback))
                            callback.CollisionCallbackStay(a, b);
                        
                        continue;
                    }
                }
                // 取Last差集是Exit
                var exit = self.LastCollisionInfos.Except(self.NowCollisionInfos);
                foreach (var pair in exit)
                {
                    CollisionObject a = pair.Item1;
                    CollisionObject b = pair.Item2;
                    // collisionA collisionB 不为空 且 unitA unitB 的TeamTag不同
                    
                    B3CollisionComponent collisionA = a.UserObject as B3CollisionComponent;
                    B3CollisionComponent collisionB = b.UserObject as B3CollisionComponent;

                    if (collisionA == null || collisionB == null)
                    {
                        if (self.Callbacks.TryGetValue(a, out var callback))
                            callback.CollisionCallbackExit(a, b);
                        
                        continue;
                    }
                    
                    TeamTag tagA = collisionA.GetParent<LSUnit>().Tag;
                    TeamTag tagB = collisionB.GetParent<LSUnit>().Tag;

                    if (tagA != tagB /*&& tagA != TeamTag.None*/)
                    {
                        if (self.Callbacks.TryGetValue(a, out var callback))
                            callback.CollisionCallbackExit(a, b);
                        
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// 需要生成的刚体
        /// </summary>
        public static void NotifyToAdd(this B3WorldComponent self, CollisionObject collision, ACollisionCallback callback = null)
        {
            self.WaitToAdds.Enqueue((collision, callback));
        }
        /// <summary>
        /// 需要销毁的刚体
        /// </summary>
        public static void NotifyToRemove(this B3WorldComponent self, CollisionObject collision)
        {
            self.WaitToRemoves.Enqueue(collision);
        }

        /// <summary>
        /// 场景中物体会使用这个来创建碰撞
        /// </summary>
        private static RigidBody AddBody(this B3WorldComponent self, Vector3[] points, Vector3 position, float mass = 0)
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
            self.NotifyToAdd(body);

            return body;
        }

        /*public static GhostObject AddTrigger(this B3WorldComponent self, CollisionShape shape, Vector3 position, ContactResultCallback callback = null)
        {
            var go = self.AddTrigger(shape, Matrix.Translation(position), callback);
            return go;
        }
        public static GhostObject AddTrigger(this B3WorldComponent self, CollisionShape shape, Matrix transform, ContactResultCallback callback = null)
        {
            PairCachingGhostObject go = new();
            go.CollisionShape = shape;
            go.WorldTransform = transform;
            self.World.AddCollisionObject(go);
            
            if (callback != null) self.Callbacks.Add(go, callback);
            return go;
        }*/
    }
}