using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = BulletSharp.Math.Vector3;

namespace ET
{
    public static class BulletEditor
    {
        [MenuItem("Tools/Generate Map Infos", false)]
        public static void GenerateMapInfos()
        {
            GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();
            List<MeshInfo> infos = new();
            foreach (GameObject go in gos)
            {
                if (!go.CompareTag("Collision_Mesh")) continue;

                MeshFilter filter = go.GetComponent<MeshFilter>();
                Rigidbody rb = go.GetComponent<Rigidbody>();
                if (filter.sharedMesh == null)
                {
                    Debug.LogWarning($"{go.name}的模型不存在");
                    continue;
                }

                float mass = (rb == null || rb.mass < 0.1) ? 0 : rb.mass;
                BulletSharp.Math.Vector3[] points = filter.sharedMesh.vertices.ToBullet();
                BulletSharp.Math.Vector3 position = go.transform.position.ToBullet();
                MeshInfo info = new() { Points = points, Position = position, Mass = mass };
                infos.Add(info);
            }

            string path = $"D:\\{SceneManager.GetActiveScene().name}.bytes";
            byte[] bytes = MemoryPackHelper.Serialize(infos);
            File.WriteAllBytes(path, bytes);
            
            Debug.Log("生成场景碰撞完成");
        }

        [MenuItem("Tools/Generate Collider Infos", false)]
        public static void GenerateColliderInfo()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Collision.unity");
            
            GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();

            List<CollisionInfo> infos = new();

            foreach (GameObject go in gos)
            {
                CollisionInfo info;
                switch (go.tag)
                {
                    case "Collision_Polygon":
                        LineRenderer renderer = go.GetComponent<LineRenderer>();
                        UnityEngine.Vector3[] points = new UnityEngine.Vector3[renderer.positionCount];
                        renderer.GetPositions(points);

                        info = new PolygonInfo()
                        {
                            Points = points.ToBullet(),
                        };
                        break;
                    case "Collision_Mesh":
                        info = new MeshInfo()
                        {
                            Points = go.GetComponent<MeshFilter>().sharedMesh.vertices.ToBullet(),
                        };
                        break;
                    case "Collision_Cube":
                        info = new CubeInfo()
                        {
                            Size = go.GetComponent<BoxCollider>().size.ToBullet(),
                        };
                        break;
                    case "Collision_Sphere":
                        info = new SphereInfo()
                        {
                            R = go.GetComponent<SphereCollider>().radius,
                        };
                        break;
                    default:
                        continue;
                }
                
                info.Id = int.Parse(go.name);
                Rigidbody rb = go.GetComponent<Rigidbody>();
                info.Mass = (rb == null || rb.mass < 0.1) ? 0 : rb.mass;
                info.Position = go.transform.position.ToBullet();
                
                infos.Add(info);
            }

            string path = $"D:\\ColliderInfos.bytes";
            byte[] bytes = MemoryPackHelper.Serialize(infos);
            File.WriteAllBytes(path, bytes);
            
            Debug.Log($"生成碰撞完成");
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