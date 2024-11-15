using UnityEngine;

namespace MarchingCube
{
    public class PerlinNoiseGenerator : ValueGenerator
    {
        private Vector3 diff;
        private float loud;
        private int seed;

        public PerlinNoiseGenerator(Vector3 diff, float loud, int seed = 0)
        {
            this.diff = diff;
            this.loud = loud;
            this.seed = seed;
        }

        public override void GenerateVertices(Vector3 area, ComputeBuffer vertices)
        {
            shader.SetVector("diff", diff);
            shader.SetFloat("loud", loud);
            shader.SetInt("seed", seed);
        
            base.GenerateVertices(area, vertices);
        }
    }
}