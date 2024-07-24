using System;
using System.Collections.Generic;
using BulletSharp;
using BulletSharp.Math;

namespace ET
{

    public static class RigidBodyHelper
    {
        public static UnityEngine.Vector3 ToUnity(Vector3 self)
        {
            return new(self.X, self.Y, self.Z);
        }
        public static Vector3 ToBullet(UnityEngine.Vector3 self)
        {
            return new Vector3(self.x, self.y, self.z);
        }
        
        public static UnityEngine.Vector3[] ToUnity(Vector3[] self)
        {
            UnityEngine.Vector3[] arr = new UnityEngine.Vector3[self.Length];
            for (int i = 0; i < self.Length; ++i)
            {
                arr[i].x = self[i].X;
                arr[i].y = self[i].Y;
                arr[i].z = self[i].Z;
            }
            return arr;
        }
        public static Vector3[] ToBullet(UnityEngine.Vector3[] self)
        {
            Vector3[] arr = new Vector3[self.Length];
            for (int i = 0; i < self.Length; ++i)
            {
                arr[i].X = self[i].x;
                arr[i].Y = self[i].y;
                arr[i].Z = self[i].z;
            }
            return arr;
        }

        public static Vector3[] ToUnity(long[] self)
        {
            if (self.Length % 3 != 0) throw new Exception("传入顶点数目不对");
            int x = 0;
            int y = 1;
            int z = 2;
            Vector3[] arr = new Vector3[self.Length / 3];
            int i = 0;

            while (x < self.Length)
            {
                arr[i].X = self[x];
                x += 3;
                arr[i].Y = self[y];
                y += 3;
                arr[i].Z = self[z];
                z += 3;
            }

            return arr;
        }
        public static UnityEngine.Vector3[] ToBullet(long[] self)
        {
            if (self.Length % 3 != 0) throw new Exception("传入顶点数目不对");
            int x = 0;
            int y = 1;
            int z = 2;
            UnityEngine.Vector3[] arr = new UnityEngine.Vector3[self.Length / 3];
            int i = 0;

            while (x < self.Length)
            {
                arr[i].x = self[x];
                x += 3;
                arr[i].y = self[y];
                y += 3;
                arr[i].z = self[z];
                z += 3;
            }

            return arr;
        }
    }
}