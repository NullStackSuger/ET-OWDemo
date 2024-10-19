namespace ET.Server
{
    public static partial class DataModifierType
    {
        [Generate(Level = "All")]
        public const int Speed = 1000;

        [Generate(Level = "All")]
        public const int Atk = 2000;
        
        [Generate(Level = "FinalConstant Extremum")]
        public const int Hp = 3000;
        
        [Generate(Level = "Constant")]
        public const int BulletCount = 4000;

        #region E技能 - 5000
        
        [Generate(Level = "All")]
        public const int ENumeric = 5000;
        
        [Generate(Level = "All")]
        public const int ECD = 5001;
        
        #endregion

        #region Q技能 - 6000

        [Generate(Level = "All")]
        public const int QNumeric = 6000;
        
        [Generate(Level = "All")]
        public const int QCD = 6001;

        #endregion

        #region C技能 - 7000

        [Generate(Level = "Constant")]
        public const int CNumeric = 7000;
        
        [Generate(Level = "Constant")]
        public const int CCD = 7001;

        #endregion

        #region 炮台 - 8000

        [Generate(Level = "Constant")]
        public const int FortNumeric = 8000;

        [Generate(Level = "Constant")]
        public const int FortRate = 8001;

        #endregion

        #region 护盾 - 9000

        [Generate(Level = "Constant")]
        public const int Shield = 9000;

        #endregion
    }
    
    public static class ModifierType
    {
        /// <summary>
        /// 常数修改器类型
        /// </summary>
        public const string Constant = "Constant";

        /// <summary>
        /// 常数修改器最大值限制
        /// </summary>
        public const string ConstantMax = "ConstantMax";

        /// <summary>
        /// 常数修改器最小值限制
        /// </summary>
        public const string ConstantMin = "ConstantMin";

        /// <summary>
        /// 百分比修改器类型
        /// </summary>
        public const string Percentage = "Percentage";

        /// <summary>
        /// 百分比修改器最大值限制
        /// </summary>
        public const string PercentageMax = "PercentageMax";

        /// <summary>
        /// 百分比修改器最小值限制
        /// </summary>
        public const string PercentageMin = "PercentageMin";

        /// <summary>
        /// 最终常数修改器类型, 如西格玛大打最大生命0.5, 通常使用后要手动移除
        /// </summary>
        public const string FinalConstant = "FinalConstant";

        /// <summary>
        /// 最终常数修改器最大值限制
        /// </summary>
        public const string FinalConstantMax = "FinalConstantMax";

        /// <summary>
        /// 最终常数修改器最小值限制
        /// </summary>
            public const string FinalConstantMin = "FinalConstantMin";

        /// <summary>
        /// 最终百分比修改器类型, 如子弹距离衰减
        /// </summary>
        public const string FinalPercentage = "FinalPercentage";

        /// <summary>
        /// 最终百分比修改器最大值限制
        /// </summary>
        public const string FinalPercentageMax = "FinalPercentageMax";

        /// <summary>
        /// 最终百分比修改器最小值限制
        /// </summary>
        public const string FinalPercentageMin = "FinalPercentageMin";

        /// <summary>
        /// 最终结果的最大值
        /// </summary>
        public const string FinalMax = "FinalMax";

        /// <summary>
        /// 最终结果的最小值
        /// </summary>
        public const string FinalMin = "FinalMin";
    }
}