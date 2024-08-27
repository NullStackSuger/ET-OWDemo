using System;

namespace ET
{
    public enum ModifierType
    {
        /// <summary>
        /// 常数修改器类型
        /// </summary>
        Constant,
        
        /// <summary>
        /// 常数修改器最大值限制
        /// </summary>
        ConstantMax,
        
        /// <summary>
        /// 常数修改器最小值限制
        /// </summary>
        ConstantMin,

        /// <summary>
        /// 百分比修改器类型
        /// </summary>
        Percentage,
        
        /// <summary>
        /// 百分比修改器最大值限制
        /// </summary>
        PercentageMax,
        
        /// <summary>
        /// 百分比修改器最小值限制
        /// </summary>
        PercentageMin,
        
        /// <summary>
        /// 最终常数修改器类型, 如西格玛大打最大生命0.5, 通常使用后要手动移除
        /// </summary>
        FinalConstant,
        
        /// <summary>
        /// 最终常数修改器最大值限制
        /// </summary>
        FinalConstantMax,
        
        /// <summary>
        /// 最终常数修改器最小值限制
        /// </summary>
        FinalConstantMin,
        
        /// <summary>
        /// 最终百分比修改器类型, 如子弹距离衰减, 通常使用后要手动移除
        /// </summary>
        FinalPercentage,
        
        /// <summary>
        /// 最终百分比修改器最大值限制
        /// </summary>
        FinalPercentageMax,
        
        /// <summary>
        /// 最终百分比修改器最小值限制
        /// </summary>
        FinalPercentageMin,
        
        /// <summary>
        /// 最终结果的最大值
        /// </summary>
        FinalMax,
        
        /// <summary>
        /// 最终结果的最小值
        /// </summary>
        FinalMin,
    }
    
    public abstract class ADataModifier : Object, IDisposable
    {
        public abstract ModifierType ModifierType { get; }

        public abstract int Key { get; }

        private long value;

        public bool NeedClear { get; set; }

        public virtual void Set(long val)
        {
            this.value = val;
        }

        public virtual long Get(DataModifierComponent self)
        {
            return this.value;
        }

        protected ADataModifier(long value)
        {
            this.value = value;
        }
        
        public virtual void Dispose()
        {
            
        }
    }

    #region Constant
    public abstract class ConstantModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.Constant;

        protected ConstantModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class ConstantMaxModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.ConstantMax;

        protected ConstantMaxModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class ConstantMinModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.ConstantMin;

        protected ConstantMinModifier(long value) : base(value)
        {
        }
    }
    #endregion

    #region Percentage
    public abstract class PercentageModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.Percentage;

        protected PercentageModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class PercentageMaxModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.PercentageMax;

        protected PercentageMaxModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class PercentageMinModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.PercentageMin;

        protected PercentageMinModifier(long value) : base(value)
        {
        }
    }
    #endregion

    #region FinalConstant
    public abstract class FinalConstantModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalConstant;

        protected FinalConstantModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class FinalConstantModifierMax : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalConstantMax;

        protected FinalConstantModifierMax(long value) : base(value)
        {
        }
    }
    
    public abstract class FinalConstantModifierMin : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalConstantMin;

        protected FinalConstantModifierMin(long value) : base(value)
        {
        }
    }
    #endregion

    #region FinalPercentage
    public abstract class FinalPercentageModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalPercentage;

        protected FinalPercentageModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class FinalPercentageMaxModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalPercentageMax;

        protected FinalPercentageMaxModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class FinalPercentageMinModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalPercentageMin;

        protected FinalPercentageMinModifier(long value) : base(value)
        {
        }
    }
    #endregion
    
    public abstract class FinalMaxModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalMax;
        
        protected FinalMaxModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class FinalMinModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalMin;
        
        protected FinalMinModifier(long value) : base(value)
        {
        }
    }
}