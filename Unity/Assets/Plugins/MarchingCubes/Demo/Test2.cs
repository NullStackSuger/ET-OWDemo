using UnityEngine;

namespace MarchingCube
{
    public class Test2 : MonoBehaviour
    {
        public Vector3 area = new Vector3Int(15, 15, 15);

        [Range(0, 15)]
        public float r = 8;

        public ComputeShader shader;

        private float4[] verices;
        private Vector3[] triangles;

        private void OnValidate()
        {
            var sphereValues = new SphereGenerator(r);
            var march = new MarchingCubes(area, sphereValues, 0);
            verices = march.Vertices();
            triangles = march.Triangles();
            
            march.Dispose();
        }

        private void OnDrawGizmos()
        {
            if (verices == null) return;

            foreach (float4 v in verices)
            {
                if (v.W <= 0)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(v, 0.1f);
                }
            }

            if (triangles == null) return;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Gizmos.DrawLine(triangles[i], triangles[i + 1]);
                Gizmos.DrawLine(triangles[i + 1], triangles[i + 2]);
                Gizmos.DrawLine(triangles[i + 2], triangles[i]);
            }
        }
    }
}