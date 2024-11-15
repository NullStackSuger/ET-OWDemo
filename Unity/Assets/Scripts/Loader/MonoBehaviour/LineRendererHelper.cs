using System;
using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererHelper : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private LineRenderer Renderer
        {
            get
            {
                if (this.lineRenderer == null)
                {
                    this.lineRenderer = this.GetComponent<LineRenderer>();
                }

                return this.lineRenderer;
            }
        }

        private UnityEngine.Vector3[] Points = Array.Empty<Vector3>();

        private void OnDrawGizmos()
        {
            int count = Renderer.positionCount;
            if (count != Points.Length)
            {
                Points = new Vector3[count];
                Renderer.GetPositions(Points);
            }

            for (int i = 0; i < count; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    Gizmos.DrawLine(Points[j], Points[i]);
                }
            }
        }
    }
}