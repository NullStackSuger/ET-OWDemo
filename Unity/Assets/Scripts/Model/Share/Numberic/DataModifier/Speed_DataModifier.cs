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
 
    public abstract class Speed_FinalMaxModifier : FinalMaxModifier
    {
        public override int Key { get; } = DataModifierType.Speed;
    }
    
    public abstract class Speed_FinalMinModifier : FinalMinModifier
    {
        public override int Key { get; } = DataModifierType.Speed;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public class Default_Speed_ConstantModifier : Speed_ConstantModifier
    {
        public static Default_Speed_ConstantModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Speed_ConstantModifier), isFromPool) as Default_Speed_ConstantModifier;
        }
    }
    
    public class Default_Speed_PercentageModifier : Speed_PercentageModifier
    {
        public static Default_Speed_PercentageModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Speed_PercentageModifier), isFromPool) as Default_Speed_PercentageModifier;
        }
    }
    
    public class Default_Speed_FinalMaxModifier : Speed_FinalMaxModifier
    {
        public static Default_Speed_FinalMaxModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Speed_FinalMaxModifier), isFromPool) as Default_Speed_FinalMaxModifier;
        }
    }
    
    public class Default_Speed_FinalMinModifier : Speed_FinalMinModifier
    {
        public static Default_Speed_FinalMinModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Speed_FinalMinModifier), isFromPool) as Default_Speed_FinalMinModifier;
        }
    }
}