using BulletSharp.Math;
using TrueSync;

/**
* @brief Extensions added by TrueSync.
**/
public static class TrueSyncExtensions 
{
    public static Vector3 ToBullet(this TSVector vector) 
    {
        return new Vector3((float)vector.x, (float)vector.y, (float)vector.z);
    }
    
    public static Vector3 ToBullet(this TSVector2 vector) 
    {
        return new Vector3((float)vector.x, (float)vector.y, 0);
    }
    
    public static Quaternion ToBullet(this TSQuaternion rot) 
    {
        return new Quaternion((float)rot.x, (float)rot.y, (float)rot.z, (float)rot.w);
    }

    public static TSVector ToTsVector(this Vector3 vector)
    {
        return new TSVector(vector.X, vector.Y, vector.Z);
    }

    public static TSVector2 ToTsVector2(this Vector3 vector)
    {
        return new TSVector2(vector.X, vector.Y);
    }
    
    public static TSQuaternion ToTSQuaternion(this Quaternion rot) 
    {
        return new TSQuaternion(rot.X, rot.Y, rot.Z, rot.W);
    }
}