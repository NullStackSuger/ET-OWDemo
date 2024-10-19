using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = BulletSharp.Math.Vector3;

namespace ET.Server
{
    public static class BulletEditor
    {
        private static CollisionInfo GenerateInfo(GameObject go)
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
                            HalfSize = go.GetComponent<BoxCollider>().size.ToBullet() * 0.5f,
                        };
                        break;
                    case "Collision_Sphere":
                        info = new SphereInfo()
                        {
                            R = go.GetComponent<SphereCollider>().radius,
                        };
                        break;
                    case "Collision_Capsule":
                        var collider = go.GetComponent<CapsuleCollider>();
                        info = new CapsuleInfo()
                        {
                            R = collider.radius,
                            Height = collider.height - 2 * collider.radius,
                        };
                        break;
                    case "Collision_RayTest":
                        var lineRenderer = go.GetComponent<LineRenderer>();
                        info = new RayTestInfo()
                        {
                            StartPos = lineRenderer.GetPosition(0).ToBullet(),
                            EndPos = lineRenderer.GetPosition(1).ToBullet(),
                        };
                        break;
                    case "Collision_Cylinder":
                        info = new CylinderInfo()
                        {
                            R = go.transform.localScale.x,
                            Height = go.transform.localScale.y,
                        };
                        break;
                    case "Untagged":
                        return null;
                    default:
                        Debug.LogError($"未实现{go.tag}");
                        return null;
                }
            
                Rigidbody rb = go.GetComponent<Rigidbody>();
                info.Mass = (rb == null || rb.mass <= 0.1) ? 0 : rb.mass;
                info.Position = go.transform.position.ToBullet();

                return info;
        }
        
        [MenuItem("Tools/Generate Map Infos", false)]
        public static void GenerateMapInfos()
        {
            GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();
            List<CollisionInfo> infos = new();
            foreach (GameObject go in gos)
            {
                var info = GenerateInfo(go);
                if (info == null) continue;
                infos.Add(info);
            }

            string path = $"D:\\{SceneManager.GetActiveScene().name}.bytes";
            byte[] bytes = MemoryPackHelper.Serialize(infos);
            File.WriteAllBytes(path, bytes);
            
            Debug.Log("生成场景碰撞完成");
        }

        [MenuItem("Tools/Generate Collider Infos", false)]
        public static void GenerateColliderInfos()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Scenes/Collision.unity");
            
            GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();

            List<CollisionInfo> infos = new();

            foreach (GameObject go in gos)
            {
                var info = GenerateInfo(go);
                if (info == null) continue;
                info.Id = int.Parse(go.name);
                
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