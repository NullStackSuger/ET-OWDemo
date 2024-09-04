using System.Linq;

namespace ET.Client
{
    [FriendOf(typeof(Room))]
    public class LSF_AI_Attack : AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            long sec = TimeInfo.Instance.ClientNow() / 1000 % 30;
            if (sec >= 20)
            {
                return 0;
            }
            
            return 1;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            await ETTask.CompletedTask;

            Room room = aiComponent.Root().GetComponent<Room>();
            TimerComponent timerComponent = aiComponent.Root().GetComponent<TimerComponent>();
            while (!cancellationToken.IsCancel())
            {
                room.Input.Button = 113;
                
                await timerComponent.WaitAsync(5000, cancellationToken);
            }

            room.Input.Button = 0;
        }
    }
}