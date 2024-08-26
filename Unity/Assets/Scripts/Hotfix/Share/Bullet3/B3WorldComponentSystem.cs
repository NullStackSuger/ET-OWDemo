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

            self.AddTrigger(new SphereShape(5), Matrix.Translation(40, 0, 0), new TriggerTestCollisionCallBack());
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
                if (collisionObject == null) continue;
                self.World.RemoveCollisionObject(collisionObject);
                if (self.Callbacks.ContainsKey(collisionObject)) self.Callbacks.Remove(collisionObject);
                collisionObject.Dispose();
            }
            
            void FixedHandler()
            {
                // 先遍历CollisionTestStart
                for (int i = self.World.CollisionObjectArray.Count - 1; i >= 0; --i)
                {
                    CollisionObject co = self.World.CollisionObjectArray[i];
                    if (co == null) continue;
                    if (!self.Callbacks.TryGetValue(co, out var callback)) continue;
                    callback.CollisionTestStart();
                }
                
                // 取Now差集是Enter
                var enter = self.NowCollisionInfos.Except(self.LastCollisionInfos);
                foreach (var pair in enter)
                {
                    CollisionObject a = pair.Item1;
                    CollisionObject b = pair.Item2;
                    
                    B3CollisionComponent collisionA = a.UserObject as B3CollisionComponent;
                    B3CollisionComponent collisionB = b.UserObject as B3CollisionComponent;
                    CollisionMaskType mask = collisionA.Mask;
                    bool needInvoke = false;
                    switch (mask)
                    {
                        case CollisionMaskType.True:
                            needInvoke = true;
                            break;
                        case CollisionMaskType.False:
                            needInvoke = false;
                            break;
                        case CollisionMaskType.Self:
                            if (collisionB == null)
                            {
                                needInvoke = false;
                                break;
                            }
                            needInvoke = collisionA.GetParent<LSUnit>().Tag == collisionB.GetParent<LSUnit>().Tag;
                            break;
                        case CollisionMaskType.SelfDiff:
                            if (collisionB == null)
                            {
                                needInvoke = true;
                                break;
                            }
                            needInvoke = collisionA.GetParent<LSUnit>().Tag != collisionB.GetParent<LSUnit>().Tag;
                            break;
                    }

                    if (needInvoke && self.Callbacks.TryGetValue(a, out var callback))
                        callback.CollisionCallbackEnter(a, b);
                }
                // 取交集是Stay
                var stay = self.LastCollisionInfos.Intersect(self.NowCollisionInfos);
                foreach (var pair in stay)
                {
                    CollisionObject a = pair.Item1;
                    CollisionObject b = pair.Item2;
                    
                    B3CollisionComponent collisionA = a.UserObject as B3CollisionComponent;
                    B3CollisionComponent collisionB = b.UserObject as B3CollisionComponent;
                    CollisionMaskType mask = collisionA.Mask;
                    bool needInvoke = false;
                    switch (mask)
                    {
                        case CollisionMaskType.True:
                            needInvoke = true;
                            break;
                        case CollisionMaskType.False:
                            needInvoke = false;
                            break;
                        case CollisionMaskType.Self:
                            if (collisionB == null)
                            {
                                needInvoke = false;
                                break;
                            }
                            needInvoke = collisionA.GetParent<LSUnit>().Tag == collisionB.GetParent<LSUnit>().Tag;
                            break;
                        case CollisionMaskType.SelfDiff:
                            if (collisionB == null)
                            {
                                needInvoke = true;
                                break;
                            }
                            needInvoke = collisionA.GetParent<LSUnit>().Tag != collisionB.GetParent<LSUnit>().Tag;
                            break;
                    }

                    if (needInvoke && self.Callbacks.TryGetValue(a, out var callback))
                        callback.CollisionCallbackStay(a, b);
                }
                // 取Last差集是Exit
                var exit = self.LastCollisionInfos.Except(self.NowCollisionInfos);
                foreach (var pair in exit)
                {
                    CollisionObject a = pair.Item1;
                    CollisionObject b = pair.Item2;
                    
                    B3CollisionComponent collisionA = a.UserObject as B3CollisionComponent;
                    B3CollisionComponent collisionB = b.UserObject as B3CollisionComponent;
                    CollisionMaskType mask = collisionA.Mask;
                    bool needInvoke = false;
                    switch (mask)
                    {
                        case CollisionMaskType.True:
                            needInvoke = true;
                            break;
                        case CollisionMaskType.False:
                            needInvoke = false;
                            break;
                        case CollisionMaskType.Self:
                            if (collisionB == null)
                            {
                                needInvoke = false;
                                break;
                            }
                            needInvoke = collisionA.GetParent<LSUnit>().Tag == collisionB.GetParent<LSUnit>().Tag;
                            break;
                        case CollisionMaskType.SelfDiff:
                            if (collisionB == null)
                            {
                                needInvoke = true;
                                break;
                            }
                            needInvoke = collisionA.GetParent<LSUnit>().Tag != collisionB.GetParent<LSUnit>().Tag;
                            break;
                    }

                    if (needInvoke && self.Callbacks.TryGetValue(a, out var callback))
                        callback.CollisionCallbackExit(a, b);
                }
                
                // 最后遍历CollisionTestFinish
                for (int i = self.World.CollisionObjectArray.Count - 1; i >= 0; --i)
                {
                    CollisionObject co = self.World.CollisionObjectArray[i];
                    if (co == null) continue;
                    if (!self.Callbacks.TryGetValue(co, out var callback)) continue;
                    callback.CollisionTestFinish();
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
        /// 获取第一个命中的目标
        /// </summary>
        public static bool RayTestFirst(this B3WorldComponent self, int collisionInfoId, out CollisionObject co)
        {
            RayTestInfo info = CollisionInfoDispatcherComponent.Instance.Infos[collisionInfoId] as RayTestInfo;
            Vector3 from = info.StartPos;
            Vector3 to = info.EndPos;
            ClosestRayResultCallback callback = new ClosestRayResultCallback(ref from, ref to);
            self.World.RayTest(from, to, callback);
            co = callback.CollisionObject;
            return callback.HasHit;
        }
        /// <summary>
        /// 获取所有命中的目标
        /// </summary>
        public static bool RayTestAll(this B3WorldComponent self, int collisionInfoId, out List<CollisionObject> cos)
        {
            RayTestInfo info = CollisionInfoDispatcherComponent.Instance.Infos[collisionInfoId] as RayTestInfo;
            Vector3 from = info.StartPos;
            Vector3 to = info.EndPos;
            AllHitsRayResultCallback callback = new AllHitsRayResultCallback(from, to);
            self.World.RayTest(from, to, callback);
            cos = callback.CollisionObjects;
            return callback.HasHit;
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

        public static GhostObject AddTrigger(this B3WorldComponent self, CollisionShape shape, Vector3 position, ACollisionCallback callback = null)
        {
            var go = self.AddTrigger(shape, Matrix.Translation(position), callback);
            return go;
        }
        public static GhostObject AddTrigger(this B3WorldComponent self, CollisionShape shape, Matrix transform, ACollisionCallback callback = null)
        {
            GhostObject go = new();
            go.CollisionShape = shape;
            go.WorldTransform = transform;
            // 不加这个标签触发器不生效
            go.CollisionFlags |= CollisionFlags.NoContactResponse;
            self.World.AddCollisionObject(go);
            
            if (callback != null) self.Callbacks.Add(go, callback);
            return go;
        }
    }
}