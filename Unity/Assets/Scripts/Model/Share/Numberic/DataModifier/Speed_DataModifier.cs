namespace ET
{
    public abstract class Speed_ConstantModifier : ConstantModifier
    {
        public override int Key { get; } = DataModifierType.Speed;

        protected Speed_ConstantModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class Speed_PercentageModifier : PercentageModifier
    {
        public override int Key { get; } = DataModifierType.Speed;

        protected Speed_PercentageModifier(long value) : base(value)
        {
        }
    }
 
    public abstract class Speed_FinalMaxModifier : FinalMaxModifier
    {
        public override int Key { get; } = DataModifierType.Speed;

        protected Speed_FinalMaxModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class Speed_FinalMinModifier : FinalMinModifier
    {
        public override int Key { get; } = DataModifierType.Speed;

        protected Speed_FinalMinModifier(long value) : base(value)
        {
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public class Default_Speed_ConstantModifier : Speed_ConstantModifier
    {
        public Default_Speed_ConstantModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Speed_PercentageModifier : Speed_PercentageModifier
    {
        public Default_Speed_PercentageModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Speed_FinalMaxModifier : Speed_FinalMaxModifier
    {
        public Default_Speed_FinalMaxModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Speed_FinalMinModifier : Speed_FinalMinModifier
    {
        public Default_Speed_FinalMinModifier(long value) : base(value)
        {
        }
    }
}