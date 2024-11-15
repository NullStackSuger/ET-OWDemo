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
        public FP OldRotation;
        public FP NewRotation;
    }

    public struct UnitChangeHeadRotation
    {
        public LSUnit Unit;
        public FP OldRotation;
        public FP NewRotation;
    }

    public struct UnitUseCast
    {
        /// <summary>
        /// 释放技能的人
        /// </summary>
        public LSUnit Owner;
        public Cast Cast;
        public string Name;
    }

    public struct UnitRemoveCast
    {
        /// <summary>
        /// 释放技能的人
        /// </summary>
        public LSUnit Owner;
        public Cast Cast;
    }

    public struct UnitUseBuff
    {
        /// <summary>
        /// 受到Buff的人
        /// </summary>
        public LSUnit Owner;
        public Buff Buff;
    }

    public struct UnitRemoveBuff
    {
        /// <summary>
        /// 受到Buff的人
        /// </summary>
        public LSUnit Owner;
        public Buff Buff;
    }

    public struct UnitOnGround
    {
        public LSUnit Unit;
        public bool OnGround;
    }

    public struct UnitChangeMoveSpeed
    {
        public LSUnit Unit;
        public float Speed;
    }
    
    public struct DataModifierChange
    {
        public LSUnit Unit;
        public int DataModifierType;
        public long Old;
        public long New;
    }
}