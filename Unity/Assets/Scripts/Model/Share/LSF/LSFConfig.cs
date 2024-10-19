namespace ET
{
    public static class LSFConfig
    {
        public const int MatchCount = 1;
        
        public const int MaxTickRate = 66;
        public const int NormalTickRate = 50;
        public const int MinTickRate = 40;

        public const int FrameCountPerSecond = 1000 / NormalTickRate;
        public const int FrameCountPerMinute = 60 * FrameCountPerSecond;

        public const float SecondPreFrame = 1f / NormalTickRate;

        public const int RecordWorldRate = FrameCountPerMinute;

        public const int G = -100;
    }
}