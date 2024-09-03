using System;
using TrueSync;

namespace ET.Client
{
    [FriendOf(typeof(Room))]
    public class LSF_AI_XunLuo : AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            long sec = TimeInfo.Instance.ClientNow() / 1000 % 15;
            if (sec < 10)
            {
                return 0;
            }
            
            // 退出时要记得把输入复原
            Room room = aiComponent.Root().GetComponent<Room>();
            room.Input.V = TSVector2.zero;
            return 1;
        }

        // 这里是类似LSFOperaComponent, 不会回滚, 用异步完全没问题
        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            await ETTask.CompletedTask;
            
            Room room = aiComponent.Root().GetComponent<Room>();
            TimerComponent timerComponent = aiComponent.Root().GetComponent<TimerComponent>();
            Random random = new();
            while (!cancellationToken.IsCancel())
            {
                switch (random.Next(0, 4))
                {
                    case 0 :
                        room.Input.V = TSVector2.up;
                        break;
                    case 1:
                        room.Input.V = TSVector2.down;
                        break;
                    case 2:
                        room.Input.V = TSVector2.left;
                        break;
                    case 3:
                        room.Input.V = TSVector2.right;
                        break;
                }

                await timerComponent.WaitAsync(1000, cancellationToken);
            }
        }
    }
}