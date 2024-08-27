
namespace ET
{
    public abstract class Hp_ConstantModifier : ConstantModifier
    {
        public override int Key { get; } = DataModifierType.Hp;

        protected Hp_ConstantModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class Hp_PercentageModifier : PercentageModifier
    {
        public override int Key { get; } = DataModifierType.Hp;

        protected Hp_PercentageModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class Hp_FinalConstantModifier : FinalConstantModifier
    {
        public override int Key { get; } = DataModifierType.Hp;
        
        protected Hp_FinalConstantModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class Hp_FinalPercentageModifier : FinalPercentageModifier
    {
        public override int Key { get; } = DataModifierType.Hp;
        
        protected Hp_FinalPercentageModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class Hp_FinalMaxModifier : FinalMaxModifier
    {
        public override int Key { get; } = DataModifierType.Hp;

        protected Hp_FinalMaxModifier(long value) : base(value)
        {
        }
    }
    
    public abstract class Hp_FinalMinModifier : FinalMinModifier
    {
        public override int Key { get; } = DataModifierType.Hp;

        protected Hp_FinalMinModifier(long value) : base(value)
        {
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public class Default_Hp_ConstantModifier : Hp_ConstantModifier
    {
        public Default_Hp_ConstantModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Hp_PercentageModifier : Hp_PercentageModifier
    {
        public Default_Hp_PercentageModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Hp_FinalConstantModifier : Hp_FinalConstantModifier
    {
        public Default_Hp_FinalConstantModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Hp_FinalPercentageModifier : Hp_FinalPercentageModifier
    {
        public Default_Hp_FinalPercentageModifier(long value) : base(value)
        {
        }
    }

    public class Default_Hp_FinalMaxModifier : Hp_FinalMaxModifier
    {
        public Default_Hp_FinalMaxModifier(long value) : base(value)
        {
        }
    }
    
    public class Default_Hp_FinalMinModifier : Hp_FinalMinModifier
    {
        public Default_Hp_FinalMinModifier(long value) : base(value)
        {
        }
    }
}