using System;
using System.Collections.Generic;
using System.Numerics;
using BulletSharp;
using BulletSharp.Math;
using MongoDB.Bson.Serialization.Attributes;
using Vector3 = BulletSharp.Math.Vector3;

namespace ET
{

    public partial class RigidBodyConfigCategory
    {
        public RigidBodyConstructionInfo Clone(int configId)
        {
            RigidBodyConfig config = this.Get(configId);
                
            DefaultMotionState motionState = new(Matrix.Translation(config.X, config.Y, config.Z));
            CollisionShape shape = null;
            switch (config.Shape)
            {
                case "Sphere":
                    shape = new SphereShape(config.Args[0]);
                    break;
                default:
                    Log.Error($"未找到对{config.Shape}的处理");
                    break;
            }
            Vector3 inertia = config.Mass == 0 ?Vector3.Zero : inertia = shape.CalculateLocalInertia(config.Mass);
            RigidBodyConstructionInfo info = new(config.Mass, motionState, shape, inertia);
                
            return info;
        }
    }
}