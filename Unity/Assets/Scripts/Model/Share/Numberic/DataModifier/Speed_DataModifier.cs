namespace ET
{
    public abstract class Speed_ConstantModifier : ConstantModifier
    {
        public override int Key { get; } = DataModifierType.Speed;
    }
    
    public abstract class Speed_PercentageModifier : PercentageModifier
    {
        public override int Key { get; } = DataModifierType.Speed;
    }
    
    public class Default_Speed_ConstantModifier : Speed_ConstantModifier
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
    
    public class Default_Speed_PercentageModifier : Speed_PercentageModifier
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