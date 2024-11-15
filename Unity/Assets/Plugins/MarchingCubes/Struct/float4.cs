using UnityEngine;

namespace MarchingCube
{
    public struct float4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public static implicit operator Vector3(float4 f)
        {
            return new Vector3(f.X, f.Y, f.Z);
        }
    }
}