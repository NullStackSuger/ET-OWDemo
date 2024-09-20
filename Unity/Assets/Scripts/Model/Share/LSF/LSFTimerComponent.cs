using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Room))]
    public class LSFTimerComponent : Entity, IAwake
    {
        /// <summary>
        /// timerId, startFrame
        /// </summary>
        public Dictionary<long, int> StartFrame = new();

        /// <summary>
        /// timerId, waitTime
        /// </summary>
        public Dictionary<long, long> WaitTime = new();
        
        public long IdGenerator;
    }

    [EntitySystemOf(typeof(LSFTimerComponent))]
    [LSEntitySystemOf(typeof(LSFTimerComponent))]
    [FriendOf(typeof(LSFTimerComponent))]
    public static partial class LSFTimerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.LSFTimerComponent self)
        {

        }

        public static bool CheckTimeOut(this ET.LSFTimerComponent self, long timerId, bool needRefresh = false)
        {
            if (!self.StartFrame.ContainsKey(timerId))
            {
                Log.Error($"{timerId}不存在, 无法检查超时");
                return false;
            }

            Room room = self.GetParent<Room>();
            int lastFrame = self.StartFrame[timerId];
            long last = room.FixedTimeCounter.FrameTime(lastFrame);
            int nowFrame = room.PredictionFrame != -1 ? room.PredictionFrame : room.AuthorityFrame;
            long now = room.FixedTimeCounter.FrameTime(nowFrame);

            bool res = (now - last) >= self.WaitTime[timerId];
            
            if (res)
            {
                if (needRefresh)
                {
                    self.Refresh(timerId);
                }
                else
                {

                    self.StartFrame.Remove(timerId);
                    self.WaitTime.Remove(timerId);
                }
            }

            return res;
        }

        public static long AddTimer(this ET.LSFTimerComponent self, long waitTime, int frame)
        {
            long id = self.GetId();
            self.StartFrame.Add(id, frame);
            self.WaitTime.Add(id, waitTime);
            return id;
        }

        public static long AddTimer(this ET.LSFTimerComponent self, long waitTime)
        {
            Room room = self.GetParent<Room>();
            int nowFrame = room.PredictionFrame != -1 ? room.PredictionFrame : room.AuthorityFrame;
            return self.AddTimer(waitTime, nowFrame);
        }

        public static void Refresh(this ET.LSFTimerComponent self, long timerId)
        {
            if (!self.StartFrame.ContainsKey(timerId))
            {
                Log.Error($"{timerId}不存在, 无法检查超时");
                return;
            }
            
            Room room = self.GetParent<Room>();
            int nowFrame = room.PredictionFrame != -1 ? room.PredictionFrame : room.AuthorityFrame;

            self.StartFrame[timerId] = nowFrame;
        }

        private static long GetId(this ET.LSFTimerComponent self)
        {
            return ++self.IdGenerator;
        }
    }
}