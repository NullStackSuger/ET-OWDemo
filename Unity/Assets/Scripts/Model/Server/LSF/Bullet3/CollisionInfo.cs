using BulletSharp.Math;
using MemoryPack;

namespace ET.Server
{
    [MemoryPackable(GenerateType.NoGenerate)]
    [MemoryPackUnion(0, typeof(PolygonInfo))]
    [MemoryPackUnion(1, typeof(MeshInfo))]
    [MemoryPackUnion(2, typeof(CubeInfo))]
    [MemoryPackUnion(3, typeof(SphereInfo))]
    [MemoryPackUnion(4, typeof(CapsuleInfo))]
    [MemoryPackUnion(5, typeof(RayTestInfo))]
    [MemoryPackUnion(6, typeof(CylinderInfo))]
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
        public Vector3 HalfSize { get; set; }
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
    
    [MemoryPackable]
    public partial class CapsuleInfo : CollisionInfo
    {
        public CapsuleInfo()
        {
            this.Tag = "Collision_Capsule";
        }

        public float R { get; set; }
        public float Height { get; set; }
    }
    
    [MemoryPackable]
    public partial class RayTestInfo : CollisionInfo
    {
        public RayTestInfo()
        {
            this.Tag = "Collision_RayTest";
        }

        public Vector3 StartPos { get; set; }
        public Vector3 EndPos { get; set; }
    }
    
    [MemoryPackable]
    public partial class CylinderInfo : CollisionInfo
    {
        public CylinderInfo()
        {
            this.Tag = "Collision_Cylinder";
        }

        public float R { get; set; }
        public float Height { get; set; }
    }
}