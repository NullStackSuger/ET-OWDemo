using UnityEngine;

namespace MarchingCube
{
    public class Test3 : MonoBehaviour
    {
        public Vector3 area = new Vector3Int(15, 15, 15);

        [Range(-1, 1)]
        public float maxValue;

        public float loud;

        public Vector3 diff = Vector3.one * 10;

        public int seed = 0;

        public ComputeShader shader;

        private float4[] verices;
        private Vector3[] triangles;

        private void OnValidate()
        {
            var perlinValues = new PerlinNoiseGenerator(diff, loud, seed);
            var march = new MarchingCubes(area, perlinValues, maxValue);
            verices = march.Vertices();
            triangles = march.Triangles();
            
            march.Dispose();
        }

        private void OnDrawGizmos()
        {
            if (verices == null) return;

            Gizmos.color = Color.gray;

            foreach (float4 v in verices)
            {
                if (v.W <= maxValue)
                {
                    Gizmos.DrawSphere(v, 0.1f);
                }
            }

            if (triangles == null) return;

            Gizmos.color = Color.green;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Gizmos.DrawLine(triangles[i], triangles[i + 1]);
                Gizmos.DrawLine(triangles[i + 1], triangles[i + 2]);
                Gizmos.DrawLine(triangles[i + 2], triangles[i]);
            }
        }
    }
}