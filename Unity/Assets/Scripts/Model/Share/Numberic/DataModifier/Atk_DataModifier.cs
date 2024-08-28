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
 
    public abstract class Atk_FinalMaxModifier : FinalMaxModifier
    {
        public override int Key { get; } = DataModifierType.Atk;
    }
    
    public abstract class Atk_FinalMinModifier : FinalMinModifier
    {
        public override int Key { get; } = DataModifierType.Atk;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public class Default_Atk_ConstantModifier : Atk_ConstantModifier
    {
        public static Default_Atk_ConstantModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Atk_ConstantModifier), isFromPool) as Default_Atk_ConstantModifier;
        }
    }
    
    public class Default_Atk_PercentageModifier : Atk_PercentageModifier
    {
        public static Default_Atk_PercentageModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Atk_PercentageModifier), isFromPool) as Default_Atk_PercentageModifier;
        }
    }
    
    public class Default_Atk_FinalMaxModifier : Atk_FinalMaxModifier
    {
        public static Default_Atk_FinalMaxModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Atk_FinalMaxModifier), isFromPool) as Default_Atk_FinalMaxModifier;
        }
    }
    
    public class Default_Atk_FinalMinModifier : Atk_FinalMinModifier
    {
        public static Default_Atk_FinalMinModifier Creat(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Default_Atk_FinalMinModifier), isFromPool) as Default_Atk_FinalMinModifier;
        }
    }
}