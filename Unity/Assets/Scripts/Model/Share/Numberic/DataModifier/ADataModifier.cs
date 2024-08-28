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
        public long Value
        {
            set
            {
                Set(value);       
            }
        }

        public bool NeedClear { get; set; }

        public virtual void Set(long val)
        {
            this.value = val;
        }

        public virtual long Get(DataModifierComponent self)
        {
            return this.value;
        }
        
        public virtual void Dispose()
        {
            
        }
    }

    #region Constant
    public abstract class ConstantModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.Constant;
    }
    
    public abstract class ConstantMaxModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.ConstantMax;
    }
    
    public abstract class ConstantMinModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.ConstantMin;
    }
    #endregion

    #region Percentage
    public abstract class PercentageModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.Percentage;
    }
    
    public abstract class PercentageMaxModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.PercentageMax;
    }
    
    public abstract class PercentageMinModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.PercentageMin;
    }
    #endregion

    #region FinalConstant
    public abstract class FinalConstantModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalConstant;
    }
    
    public abstract class FinalConstantModifierMax : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalConstantMax;
    }
    
    public abstract class FinalConstantModifierMin : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalConstantMin;
    }
    #endregion

    #region FinalPercentage
    public abstract class FinalPercentageModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalPercentage;
    }
    
    public abstract class FinalPercentageMaxModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalPercentageMax;
    }
    
    public abstract class FinalPercentageMinModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalPercentageMin;
    }
    #endregion
    
    public abstract class FinalMaxModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalMax;
    }
    
    public abstract class FinalMinModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalMin;
    }
}