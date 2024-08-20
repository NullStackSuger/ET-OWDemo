using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using BulletSharp;
using BulletSharp.Math;
using MongoDB.Bson.Serialization.Attributes;
using Vector3 = BulletSharp.Math.Vector3;

namespace ET
{

    public partial class RigidBodyConfigCategory
    {
        private readonly Dictionary<int, ColliderInfo> Infos = new();
        
        public override void EndInit()
        {
            base.EndInit();
            
            string path = $"D:\\ColliderInfos.bytes";
            if (!File.Exists(path)) return;
            
            byte[] bytes = File.ReadAllBytes(path);
            List<ColliderInfo> infos = MemoryPackHelper.Deserialize(typeof(List<ColliderInfo>), bytes, 0, bytes.Length) as List<ColliderInfo>;

            foreach (ColliderInfo info in infos)
            {
                this.Infos.Add(info.Id, info);
            }
        }

        public RigidBodyConstructionInfo Clone(int configId)
        {
            ColliderInfo info = this.Infos[configId];
                
            DefaultMotionState motionState = new(Matrix.Translation(info.Position));
            CollisionShape shape = null;
            switch (info.Tag)
            {
                case "Collider_Polygon":
                    shape = new ConvexHullShape(((PolygonInfo)info).Points);
                    ((ConvexHullShape)shape).InitializePolyhedralFeatures();
                    break;
                case "Collider_Mesh":
                    shape = new ConvexHullShape(((MeshInfo)info).Points);
                    ((ConvexHullShape)shape).InitializePolyhedralFeatures();
                    break;
                case "Collider_Cube":
                    shape = new BoxShape(((CubeInfo)info).Size);
                    break;
                case "Collider_Sphere":
                    shape = new SphereShape(((SphereInfo)info).R);
                    break;
                default:
                    Log.Error($"未找到对{info.Tag}的处理");
                    break;
            }
            Vector3 inertia = info.Mass == 0 ? Vector3.Zero : inertia = shape.CalculateLocalInertia(info.Mass);
            RigidBodyConstructionInfo rbInfo = new(info.Mass, motionState, shape, inertia);
                
            return rbInfo;
        }
    }
}