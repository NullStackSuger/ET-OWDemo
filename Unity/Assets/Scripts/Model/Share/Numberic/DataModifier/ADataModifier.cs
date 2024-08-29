using System;

namespace ET
{
    public abstract class ADataModifier : Object, IDisposable
    {
        public abstract int ModifierType { get; }

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
        public override int ModifierType { get; } = ET.ModifierType.Constant;
    }
    
    public abstract class ConstantMaxModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.ConstantMax;
    }
    
    public abstract class ConstantMinModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.ConstantMin;
    }
    #endregion

    #region Percentage
    public abstract class PercentageModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.Percentage;
    }
    
    public abstract class PercentageMaxModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.PercentageMax;
    }
    
    public abstract class PercentageMinModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.PercentageMin;
    }
    #endregion

    #region FinalConstant
    public abstract class FinalConstantModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.FinalConstant;
    }
    
    public abstract class FinalConstantModifierMax : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.FinalConstantMax;
    }
    
    public abstract class FinalConstantModifierMin : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.FinalConstantMin;
    }
    #endregion

    #region FinalPercentage
    public abstract class FinalPercentageModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.FinalPercentage;
    }
    
    public abstract class FinalPercentageMaxModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.FinalPercentageMax;
    }
    
    public abstract class FinalPercentageMinModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.FinalPercentageMin;
    }
    #endregion
    
    public abstract class FinalMaxModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.FinalMax;
    }
    
    public abstract class FinalMinModifier : ADataModifier
    {
        public override int ModifierType { get; } = ET.ModifierType.FinalMin;
    }
}