using UnityEngine;

namespace MarchingCube
{
    public class SphereGenerator : ValueGenerator
    {
        private float radius;

        public SphereGenerator(float radius)
        {
            this.radius = radius;
        }

        public override void GenerateVertices(Vector3 area, ComputeBuffer vertices)
        {
            shader.SetFloat("radius", radius);
            base.GenerateVertices(area, vertices);
        }
    }
}