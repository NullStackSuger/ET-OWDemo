using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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

                BulletSharp.Math.Vector3[] points = RigidBodyHelper.ToBullet(filter.sharedMesh.vertices);
                BulletSharp.Math.Vector3 position = RigidBodyHelper.ToBullet(go.transform.position);
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
    }
}