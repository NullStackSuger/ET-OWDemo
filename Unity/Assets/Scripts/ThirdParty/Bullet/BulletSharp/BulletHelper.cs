using BulletSharp.Math;
using TrueSync;

namespace BulletSharp
{
    public static class BulletHelper
    {
        public static Vector3 ToVector(this TSVector vector)
        {
            return new Vector3((float)vector.x, (float)vector.y, (float)vector.z);
        }
        
        public static Vector3 ToVector(this TSVector2 vector)
        {
            return new Vector3((float)vector.x, (float)vector.y, 0);
        }
        
        public static TSVector ToTSVector(this Vector3 vector)
        {
            return new TSVector(vector.X, vector.Y, vector.Z);
        }
        
        public static TSVector2 ToTSVector2(this Vector3 vector)
        {
            return new TSVector2(vector.X, vector.Y);
        }

        public static Quaternion ToQuaternion(this TSQuaternion rot)
        {
            return new Quaternion((float)rot.x, (float)rot.y, (float)rot.z, (float)rot.w);
        }
        
        public static Quaternion ToQuaternion(this TSMatrix mat)
        {
            return TSQuaternion.CreateFromMatrix(mat).ToQuaternion();
        }
        
        public static TSQuaternion ToTSQuaternion(this Quaternion rot)
        {
            return new TSQuaternion(rot.X, rot.Y, rot.Z, rot.W);
        }

        public static TSMatrix ToTsMatrix(this Quaternion rot)
        {
            return TSMatrix.CreateFromQuaternion(rot.ToTSQuaternion());
        }
    }
}