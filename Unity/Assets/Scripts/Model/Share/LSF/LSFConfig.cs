namespace ET
{
    public static class LSFConfig
    {
        public const int MatchCount = 2;
        
        public const int MaxTickRate = 66;
        public const int NormalTickRate = 50;
        public const int MinTickRate = 40;

        public const int FrameCountPreSecond = 1000 / NormalTickRate;
        public const int FrameCountPreMinute = 60 * FrameCountPreSecond;

        public const int RecordWorldRate = FrameCountPreMinute;

        public const int Speed = 6;
    }
}