using UnityEngine;

namespace MarchingCube
{
    /// <summary>
    /// 用于设置Cell顶点的值
    /// </summary>
    public abstract class ValueGenerator
    {
        protected ComputeShader shader;
        protected int kernel;

        protected ValueGenerator()
        {
            shader = Resources.Load<ComputeShader>($"Generator/{this.GetType().Name}");
            kernel = shader.FindKernel("CSMain");
        }

        ~ValueGenerator()
        {
            // 释放一些东西, 因为在编辑器里使用, 不释放可能有问题
        }

        public virtual void GenerateVertices(Vector3 area, ComputeBuffer vertices)
        {
            shader.SetBuffer(kernel, "vertices", vertices);
            shader.SetVector("area", area);

            shader.GetKernelThreadGroupSizes(kernel, out var x, out var y, out var z);
            shader.Dispatch(kernel, Mathf.CeilToInt(area.x / x), Mathf.CeilToInt(area.y / y), Mathf.CeilToInt(area.z / z));
        }
    }
}