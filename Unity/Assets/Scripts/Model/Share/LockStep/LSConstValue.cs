namespace ET
{
    public static class LSConstValue
    {
        public const int MatchCount = 1;
        public const int UpdateInterval = 50;
        /// <summary>
        /// 每秒多少帧
        /// </summary>
        public const int FrameCountPerSecond = 1000 / UpdateInterval;
        /// <summary>
        /// 每分钟有多少帧
        /// </summary>
        public const int SaveLSWorldFrameCount = 60 * FrameCountPerSecond;
    }
}