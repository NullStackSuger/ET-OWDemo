using System.Collections.Generic;
using BulletSharp.Math;
using MemoryPack;

namespace ET
{
    [MemoryPackable]
    // TODO: 这里应该类似MongoRegister, 自动注册
    // 但是没找到不用泛型的注册方式, 可能要自己写一个
    [MemoryPackUnion(0, typeof(PolygonInfo))]
    [MemoryPackUnion(1, typeof(MeshInfo))]
    [MemoryPackUnion(2, typeof(CubeInfo))]
    [MemoryPackUnion(3, typeof(SphereInfo))]
    public abstract partial class CollisionInfo : Object
    {
        public int Id { get; set; }

        /// <summary>
        /// 对于环境物体, 表示位置
        /// 对于特殊物体, 表示偏移
        /// </summary>
        public Vector3 Position { get; set; }
        public float Mass { get; set; }
        public string Tag { get; set; }
    }
    
    [MemoryPackable]
    public partial class PolygonInfo : CollisionInfo
    {
        public PolygonInfo()
        {
            this.Tag = "Collision_Polygon";
        }
        
        public Vector3[] Points { get; set; }
    }
    
    [MemoryPackable]
    public partial class MeshInfo : CollisionInfo
    {
        public MeshInfo()
        {
            this.Tag = "Collision_Mesh";
        }
        
        public Vector3[] Points { get; set; }
    }
    
    [MemoryPackable]
    public partial class CubeInfo : CollisionInfo
    {
        public CubeInfo()
        {
            this.Tag = "Collision_Cube";
        }
        public Vector3 Size { get; set; }
    }
    
    [MemoryPackable]
    public partial class SphereInfo : CollisionInfo
    {
        public SphereInfo()
        {
            this.Tag = "Collision_Sphere";
        }
        public float R { get; set; }
    }
}