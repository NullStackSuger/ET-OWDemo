using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using BulletSharp;
using BulletSharp.Math;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using Vector3 = BulletSharp.Math.Vector3;

namespace ET
{

    public partial class CollisionConfigCategory
    {
        private readonly Dictionary<int, CollisionInfo> Infos = new();
        
        public override void EndInit()
        {
            base.EndInit();
            
            string path = $"D:\\ColliderInfos.bytes";
            if (!File.Exists(path)) return;
            
            byte[] bytes = File.ReadAllBytes(path);
            List<CollisionInfo> infos = MemoryPackHelper.Deserialize(typeof(List<CollisionInfo>), bytes, 0, bytes.Length) as List<CollisionInfo>;

            foreach (CollisionInfo info in infos)
            {
                this.Infos.Add(info.Id, info);
            }
        }

        public CollisionObject Clone(int configId)
        {
            CollisionConfig config = CollisionConfigCategory.Instance.Get(configId);
            CollisionInfo info = this.Infos[configId];
            
            CollisionShape shape = null;
            switch (info.Tag)
            {
                case "Collision_Polygon":
                    shape = new ConvexHullShape(((PolygonInfo)info).Points);
                    ((ConvexHullShape)shape).InitializePolyhedralFeatures();
                    break;
                case "Collision_Mesh":
                    shape = new ConvexHullShape(((MeshInfo)info).Points);
                    ((ConvexHullShape)shape).InitializePolyhedralFeatures();
                    break;
                case "Collision_Cube":
                    shape = new BoxShape(((CubeInfo)info).Size);
                    break;
                case "Collision_Sphere":
                    shape = new SphereShape(((SphereInfo)info).R);
                    break;
                default:
                    Log.Error($"未找到对{info.Tag}的处理");
                    break;
            }
            
            if (config.IsTrigger == 1)
            {
                GhostObject go = new GhostObject();
                go.CollisionShape = shape;
                go.WorldTransform = Matrix.Translation(info.Position);
                go.CollisionFlags |= CollisionFlags.NoContactResponse;

                return go;
            }
            else
            {
                DefaultMotionState motionState = new(Matrix.Translation(info.Position));
                Vector3 inertia = info.Mass == 0 ? Vector3.Zero : inertia = shape.CalculateLocalInertia(info.Mass);
                RigidBodyConstructionInfo rbInfo = new(info.Mass, motionState, shape, inertia);
                RigidBody body = new RigidBody(rbInfo);
                
                if (info.Mass == 0)
                {
                    body.CollisionFlags |= CollisionFlags.KinematicObject;
                    body.ActivationState = ActivationState.DisableDeactivation;
                }

                return body;
            }
        }
    }
}