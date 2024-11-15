using BulletSharp;
using BulletSharp.Math;

namespace ET.Server
{
    [ComponentOf(typeof(B3CollisionComponent))]
    public class P2PConstraintComponent : LSEntity, IAwake<Vector3, long>, IDestroy
    {
        public Point2PointConstraint Constraint { get; set; }
        
        public long Length { get; set; }
        
        public Vector3 Point { get; set; } // 原点
    }
}