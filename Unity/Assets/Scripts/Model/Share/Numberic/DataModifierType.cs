namespace ET
{
    public static class DataModifierType
    {
        public const int Min = 0000;
        
        public const int Speed = 0001;

        public const int Atk = 0002;
        
        public const int Hp = 0003;
        
        public const int Max = 10000;
    }
    
    public static class ModifierType
    {
        /// <summary>
        /// 常数修改器类型
        /// </summary>
        public const int Constant = 0001;
        
        /// <summary>
        /// 常数修改器最大值限制
        /// </summary>
        public const int ConstantMax = 0002;
        
        /// <summary>
        /// 常数修改器最小值限制
        /// </summary>
        public const int ConstantMin = 0003;

        /// <summary>
        /// 百分比修改器类型
        /// </summary>
        public const int Percentage = 0004;

        /// <summary>
        /// 百分比修改器最大值限制
        /// </summary>
        public const int PercentageMax = 0005;
        
        /// <summary>
        /// 百分比修改器最小值限制
        /// </summary>
        public const int PercentageMin = 0006;
        
        /// <summary>
        /// 最终常数修改器类型, 如西格玛大打最大生命0.5, 通常使用后要手动移除
        /// </summary>
        public const int FinalConstant = 0007;
        
        /// <summary>
        /// 最终常数修改器最大值限制
        /// </summary>
        public const int FinalConstantMax = 0008;
        
        /// <summary>
        /// 最终常数修改器最小值限制
        /// </summary>
        public const int FinalConstantMin = 0009;
        
        /// <summary>
        /// 最终百分比修改器类型, 如子弹距离衰减, 通常使用后要手动移除
        /// </summary>
        public const int FinalPercentage = 0010;
        
        /// <summary>
        /// 最终百分比修改器最大值限制
        /// </summary>
        public const int FinalPercentageMax = 0011;
        
        /// <summary>
        /// 最终百分比修改器最小值限制
        /// </summary>
        public const int FinalPercentageMin = 0012;
        
        /// <summary>
        /// 最终结果的最大值
        /// </summary>
        public const int FinalMax = 0013;
        
        /// <summary>
        /// 最终结果的最小值
        /// </summary>
        public const int FinalMin = 0014;
    }
}