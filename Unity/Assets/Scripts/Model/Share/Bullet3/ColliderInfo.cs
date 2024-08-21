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
    public abstract partial class ColliderInfo : Object
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
    public partial class PolygonInfo : ColliderInfo
    {
        public PolygonInfo()
        {
            this.Tag = "Collider_Polygon";
        }
        
        public Vector3[] Points { get; set; }
    }
    
    [MemoryPackable]
    public partial class MeshInfo : ColliderInfo
    {
        public MeshInfo()
        {
            this.Tag = "Collider_Mesh";
        }
        
        public Vector3[] Points { get; set; }
    }
    
    [MemoryPackable]
    public partial class CubeInfo : ColliderInfo
    {
        public CubeInfo()
        {
            this.Tag = "Collider_Cube";
        }
        public Vector3 Size { get; set; }
    }
    
    [MemoryPackable]
    public partial class SphereInfo : ColliderInfo
    {
        public SphereInfo()
        {
            this.Tag = "Collider_Sphere";
        }
        public float R { get; set; }
    }
}