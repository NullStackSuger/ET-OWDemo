using BulletSharp.Math;
using TrueSync;

namespace ET
{
    public struct AfterCreatRobot
    {
        
    }
    
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
        /// <summary>
        /// 释放技能的人
        /// </summary>
        public LSUnit Unit;
        public Cast Cast;
    }

    public struct UnitRemoveCast
    {
        /// <summary>
        /// 释放技能的人
        /// </summary>
        public LSUnit Unit;
        public Cast Cast;
    }

    public struct UnitUseBuff
    {
        /// <summary>
        /// 受到Buff的人
        /// </summary>
        public LSUnit Unit;
        public Buff Buff;
    }

    public struct UnitRemoveBuff
    {
        /// <summary>
        /// 受到Buff的人
        /// </summary>
        public LSUnit Unit;
        public Buff Buff;
    }
}