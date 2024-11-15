using System;

namespace ET.Client
{
    [EntitySystemOf(typeof(ReplayUpdateComponent))]
    [FriendOf(typeof(ReplayUpdateComponent))]
    public static partial class ReplayUpdateComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ReplayUpdateComponent self)
        {

        }

        [EntitySystem]
        private static void Update(this ReplayUpdateComponent self)
        {
            Room room = self.GetParent<Room>();
            long now = TimeInfo.Instance.ServerNow();

            while (true)
            {
                // 播放完了
                if (room.AuthorityFrame + 1 >= room.Replay.DeltaEvents.Count) return;

                // 检查时间是否通过
                // 放循环里 比如一次Update是0.4s 一帧0.2s 那一次Update就要执行2帧
                long next = room.FixedTimeCounter.FrameTime(room.AuthorityFrame + 1);
                if (now < next) return;

                ++room.AuthorityFrame;
                
                Scene scene = self.Root();
                
                // 设置输入
                OneFrameInputs inputs = room.Replay.FrameInputs[room.AuthorityFrame];
                room.PredictionWorld.Update(inputs);
                
                // 设置脏数据
                MailBoxComponent mailBox = scene.GetComponent<MailBoxComponent>();
                OneFrameDeltaEvents deltaEvents = room.Replay.DeltaEvents[room.AuthorityFrame];
                foreach (var items in deltaEvents.Events.Values)
                {
                    foreach (MessageObject delta in items)
                    {
                        // 第1个参数地址其实可以填null, 里面不会使用Address
                        mailBox.Add(scene.Fiber.Address, delta);   
                    }
                }

                // 单次update时间>5ms 就留到下次update再做
                // 避免单次update时间太长 卡住
                long now2 = TimeInfo.Instance.ServerNow();
                if (now2 > now + 5) break;
            }
        }

        public static void ChangeReplaySpeed(this ReplayUpdateComponent self)
        {
            Room room = self.GetParent<Room>();
            if (self.ReplaySpeed == 8) self.ReplaySpeed = 1;
            else self.ReplaySpeed *= 2;
            room.FixedTimeCounter.ChangeInterval(LSFConfig.NormalTickRate / self.ReplaySpeed, room.AuthorityFrame);
        }
    }
}