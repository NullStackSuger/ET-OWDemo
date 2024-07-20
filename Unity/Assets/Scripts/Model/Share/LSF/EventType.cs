using BulletSharp.Math;
using TrueSync;

namespace ET
{
    public struct UnitChangePosition
    {
        public LSUnit Unit;
        public TSVector OldPosition;
        public TSVector NewPosition;
    }
    public struct UnitChangeRotation
    {
        public LSUnit Unit;
        public TSQuaternion OldRotation;
        public TSQuaternion NewRotation;
    }

    public struct UnitUseCast
    {
        public LSUnit Unit;
        public Cast Cast;
    }
}