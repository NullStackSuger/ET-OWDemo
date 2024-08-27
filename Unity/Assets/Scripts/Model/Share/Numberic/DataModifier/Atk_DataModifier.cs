namespace ET
{
    public class Atk_ConstantModifier : ConstantModifier
    {
        public override int Key { get; } = DataModifierType.Atk;

        protected Atk_ConstantModifier(long value) : base(value)
        {
        }
    }
    
    public class Atk_PercentageModifier : PercentageModifier
    {
        public override int Key { get; } = DataModifierType.Atk;

        protected Atk_PercentageModifier(long value) : base(value)
        {
        }
    }
 
    public abstract class Atk_FinalMaxModifier : FinalMaxModifier
    {
        public override int Key { get; } = DataModifierType.Atk;

        protected Atk_FinalMaxModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class Atk_FinalMinModifier : FinalMinModifier
    {
        public override int Key { get; } = DataModifierType.Atk;

        protected Atk_FinalMinModifier(long value) : base(value)
        {
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public class Default_Atk_ConstantModifier : Atk_ConstantModifier
    {
        public Default_Atk_ConstantModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Atk_PercentageModifier : Atk_PercentageModifier
    {
        public Default_Atk_PercentageModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Atk_FinalMaxModifier : Atk_FinalMaxModifier
    {
        public Default_Atk_FinalMaxModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Atk_FinalMinModifier : Atk_FinalMinModifier
    {
        public Default_Atk_FinalMinModifier(long value) : base(value)
        {
        }
    }
}