namespace ET
{
    public abstract class Hp_ConstantModifier : ConstantModifier
    {
        public override int Key { get; } = DataModifierType.Hp;
    }
    
    public abstract class Hp_PercentageModifier : PercentageModifier
    {
        public override int Key { get; } = DataModifierType.Hp;
    }
    
    public class Default_Hp_ConstantModifier : Hp_ConstantModifier
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
    
    public class Default_Hp_PercentageModifier : Hp_PercentageModifier
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