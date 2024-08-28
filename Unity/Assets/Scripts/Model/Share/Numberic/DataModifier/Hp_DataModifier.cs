
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
    
    public abstract class Hp_FinalConstantModifier : FinalConstantModifier
    {
        public override int Key { get; } = DataModifierType.Hp;
    }
    
    public abstract class Hp_FinalPercentageModifier : FinalPercentageModifier
    {
        public override int Key { get; } = DataModifierType.Hp;
    }
    
    public abstract class Hp_FinalMaxModifier : FinalMaxModifier
    {
        public override int Key { get; } = DataModifierType.Hp;
    }
    
    public abstract class Hp_FinalMinModifier : FinalMinModifier
    {
        public override int Key { get; } = DataModifierType.Hp;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public class Default_Hp_ConstantModifier : Hp_ConstantModifier
    {
        public static Default_Hp_ConstantModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Hp_ConstantModifier), isFromPool) as Default_Hp_ConstantModifier;
        }
    }
    
    public class Default_Hp_PercentageModifier : Hp_PercentageModifier
    {
        public static Default_Hp_PercentageModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Hp_PercentageModifier), isFromPool) as Default_Hp_PercentageModifier;
        }
    }
    
    public class Default_Hp_FinalConstantModifier : Hp_FinalConstantModifier
    {
        public static Default_Hp_FinalConstantModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Hp_FinalConstantModifier), isFromPool) as Default_Hp_FinalConstantModifier;
        }
    }
    
    public class Default_Hp_FinalPercentageModifier : Hp_FinalPercentageModifier
    {
        public static Default_Hp_FinalPercentageModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Hp_FinalPercentageModifier), isFromPool) as Default_Hp_FinalPercentageModifier;
        }
    }

    public class Default_Hp_FinalMaxModifier : Hp_FinalMaxModifier
    {
        public static Default_Hp_FinalMaxModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Hp_FinalMaxModifier), isFromPool) as Default_Hp_FinalMaxModifier;
        }
    }
    
    public class Default_Hp_FinalMinModifier : Hp_FinalMinModifier
    {
        public static Default_Hp_FinalMinModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Hp_FinalMinModifier), isFromPool) as Default_Hp_FinalMinModifier;
        }
    }
}