using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = BulletSharp.Math.Vector3;

namespace ET
{
    public static class RigidBodyEditor
    {
        [MenuItem("Tools/Generate Map Infos", false)]
        public static void GenerateMeshInfos()
        {
            ET_MeshCollider[] colliders = UnityEngine.Object.FindObjectsOfType<ET_MeshCollider>();
            List<MeshInfo> infos = new();
            foreach (ET_MeshCollider collider in colliders)
            {
                GameObject go = collider.gameObject;
                MeshFilter filter = go.GetComponent<MeshFilter>();
                if (filter.sharedMesh == null) return;

                BulletSharp.Math.Vector3[] points = filter.sharedMesh.vertices.ToBullet();
                BulletSharp.Math.Vector3 position = go.transform.position.ToBullet();
                float mass = collider.Mass;
                MeshInfo info = new() { Points = points, Position = position, Mass = mass };
                infos.Add(info);
            }

            if (infos.Count <= 0) return;

            string path = $"D:\\{SceneManager.GetActiveScene().name}.bytes";
            byte[] bytes = MemoryPackHelper.Serialize(infos);
            File.WriteAllBytes(path, bytes);
            
            Debug.Log("生成场景碰撞完成");
        }

        private static Vector3[] ToBullet(this UnityEngine.Vector3[] self)
        {
            Vector3[] arr = new Vector3[self.Length];
            for (int i = 0; i < self.Length; i++)
            {
                UnityEngine.Vector3 v = self[i];
                arr[i].X = v.x;
                arr[i].Y = v.y;
                arr[i].Z = v.z;
            }
            return arr;
        }

        private static Vector3 ToBullet(this UnityEngine.Vector3 self)
        {
            return new Vector3(self.x, self.y, self.z);
        }
    }
}