using BulletSharp.Math;
using MemoryPack;

namespace ET
{
    [MemoryPackable]
    public partial class MeshInfo : Object
    {
        public Vector3[] Points { get; set; }
        public Vector3 Position { get; set; }
        public float Mass { get; set; }
    }
}