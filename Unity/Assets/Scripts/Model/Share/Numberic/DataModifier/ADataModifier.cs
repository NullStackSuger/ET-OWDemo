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
        /// 百分比修改器类型
        /// </summary>
        Percentage,
        
        /// <summary>
        /// 最终常数修改器类型, 如西格玛大打最大生命0.5, 通常使用后要手动移除
        /// </summary>
        FinalConstant,
        
        /// <summary>
        /// 最终百分比修改器类型, 如子弹距离衰减, 通常使用后要手动移除
        /// </summary>
        FinalPercentage,
    }
    
    public abstract class ADataModifier : Object, IDisposable
    {
        public abstract ModifierType ModifierType { get; }

        public abstract int Key { get; }

        protected long Value;

        public virtual void Set(long value)
        {
            this.Value = value;
        }

        public virtual long Get()
        {
            return this.Value;
        }
        
        public virtual void Dispose()
        {
            
        }
    }

    public abstract class ConstantModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.Constant;
    }

    public abstract class PercentageModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.Percentage;
    }

    public abstract class FinalConstantModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalConstant;
    }

    public abstract class FinalPercentageModifier : ADataModifier
    {
        public override ModifierType ModifierType { get; } = ModifierType.FinalPercentage;
    }
}