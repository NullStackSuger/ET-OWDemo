using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarchingCube
{
    /// <summary>
    /// 管理所有Cells
    /// </summary>
    public class MarchingCubes : IDisposable
    {
        /// <summary>
        /// 所有Cell的顶点的位置和值
        /// </summary>
        private ComputeBuffer verticesBuffer;

        /// <summary>
        /// 所有要生成的三角面片(3个点)
        /// </summary>
        private ComputeBuffer trianglesBuffer;

        private ComputeShader shader;

        private List<float4> vertices;

        private List<Vector3> triangles;

        public MarchingCubes(Vector3 area, ValueGenerator values, float maxValue)
        {
            // 初始化
            shader = Resources.Load<ComputeShader>(nameof(MarchingCubes));

            int verticesCount = ((int)area.x + 1) * ((int)area.y + 1) * ((int)area.z + 1);
            int cellCount = (int)area.x * (int)area.y * (int)area.z;
            verticesBuffer = new ComputeBuffer(verticesCount, 4 * 4 /*float4*/);
            trianglesBuffer = new ComputeBuffer(cellCount * 5, 4 * 3 * 3 /*Triangle*/, ComputeBufferType.Append);

            // 获取顶点坐标和值
            values.GenerateVertices(area, verticesBuffer);

            trianglesBuffer.SetCounterValue(0); // Clear之前的数据, 不改长度

            // 设置属性
            shader.SetBuffer(0, "triangles", trianglesBuffer);
            shader.SetBuffer(0, "vertices", verticesBuffer);
            shader.SetVector("area", new Vector4(area.x, area.y, area.z, 0));
            shader.SetFloat("maxValue", maxValue);

            shader.GetKernelThreadGroupSizes(0, out var x, out var y, out var z);
            // 调用, 正常2个int相除会有问题, 但这里能确保是整数
            shader.Dispatch(0, Mathf.CeilToInt(area.x / x), Mathf.CeilToInt(area.y / y), Mathf.CeilToInt(area.z / z));

            // 获取结果
            #region 获取三角面片
            // 获取面片数量
            // 注意是CopyCount, 复制数量, 我眼瞎找了好久
            ComputeBuffer countBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
            ComputeBuffer.CopyCount(trianglesBuffer, countBuffer, 0);
            int[] countArr = { 0 };
            countBuffer.GetData(countArr);
            countBuffer?.Release();
            countBuffer?.Dispose();
            int triangleCount = countArr[0];
            
            // 获取面片
            triangles = new List<Vector3>(triangleCount * 3);
            Triangle[] trianglesArr = new Triangle[triangleCount];
            trianglesBuffer.GetData(trianglesArr);
            foreach (Triangle triangle in trianglesArr)
            {
                triangles.Add(triangle.a);
                triangles.Add(triangle.b);
                triangles.Add(triangle.c);
            }
            #endregion

            #region 获取顶点
            vertices = new List<float4>(verticesCount);
            float4[] verticesArr = new float4[verticesCount];
            verticesBuffer.GetData(verticesArr);
            foreach (float4 vertex in verticesArr)
            {
                vertices.Add(vertex);
            }
            #endregion
        }

        ~MarchingCubes()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            verticesBuffer?.Release();
            trianglesBuffer?.Release();
            
            verticesBuffer?.Dispose();
            trianglesBuffer?.Dispose();
        }

        public float4[] Vertices()
        {
            return vertices.ToArray();
        }

        public Vector3[] Triangles()
        {
            return triangles.ToArray();
        }
    }
}