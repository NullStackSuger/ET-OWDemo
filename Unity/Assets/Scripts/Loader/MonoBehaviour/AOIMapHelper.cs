using UnityEngine;  
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using Vector2 = System.Numerics.Vector2;

namespace ET
{
    public class AOIMapHelper : MonoBehaviour
    {
        private static readonly Vector3 CellSize = new(10, 0.01f, 10);
        private static readonly Vector3 Origin = new(CellSize.x * 0.5f, 0, CellSize.z * 0.5f);
        
        [ShowInInspector]
        private List<Vector2> points = new List<Vector2>();
        [ShowInInspector]
        private Color selectedColor = Color.green;
        [ShowInInspector]
        private Color outlineColor = Color.yellow;
        [ShowInInspector]
        private float height = 0;
        
        [Button]
        public void Generate()
        {
            List<Vector2> result = new List<Vector2>();
            foreach (Vector2 point in this.points)
            {
                Vector2 vector = new Vector2(point.X, point.Y);
                if (result.Contains(vector)) continue;
                result.Add(vector);
            }
            
            if (result.Count <= 0) return;
            
            string path = $"D:\\AOI{SceneManager.GetActiveScene().name}.bytes";
            byte[] bytes = MemoryPackHelper.Serialize(result);
            File.WriteAllBytes(path, bytes);
            
            Debug.Log("生成场景AOI完成");
        }

        private void OnDrawGizmos()
        {
            Vector3 center = new();
            center.y = this.height;
            foreach (Vector2 point in this.points)
            {
                center.x = point.X * CellSize.x + Origin.x;
                center.z = point.Y * CellSize.z + Origin.z;
                
                Gizmos.color = selectedColor;
                Gizmos.DrawCube(center, CellSize);
                
                Gizmos.color = outlineColor;
                Gizmos.DrawWireCube(center, CellSize);
            }
        }
    }
}