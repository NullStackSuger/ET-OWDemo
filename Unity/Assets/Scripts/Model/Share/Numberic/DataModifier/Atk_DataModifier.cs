namespace ET
{
    public class Atk_ConstantModifier : ConstantModifier
    {
        public override int Key { get; } = DataModifierType.Atk;
    }
    
    public class Atk_PercentageModifier : PercentageModifier
    {
        public override int Key { get; } = DataModifierType.Atk;
    }
    
    public class Default_Atk_ConstantModifier : Atk_ConstantModifier
    {
        public override void Set(long value)
        {
            base.Set(value);
        }

        public override long Get()
        {
            return base.Get();
        }
    }
    
    public class Default_Atk_PercentageModifier : Atk_PercentageModifier
    {
        public override void Set(long value)
        {
            base.Set(value);
        }

        public override long Get()
        {
            return base.Get();
        }
    }
}