using System.Linq;

namespace ET.Client
{
    [FriendOf(typeof(Room))]
    public class LSF_AI_Attack : AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            long sec = TimeInfo.Instance.ClientNow() / 1000 % 15;
            if (sec >= 10)
            {
                return 0;
            }
            
            Room room = aiComponent.Root().GetComponent<Room>();
            room.Input.Button = 0;
            return 1;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            await ETTask.CompletedTask;

            Room room = aiComponent.Root().GetComponent<Room>();
            //room.Input.Button = 113;
        }
    }
}