using TrueSync;

namespace ET.Client
{
    [FriendOf(typeof(Room))]
    public class LSF_AI_XunLuo : AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            return 0;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            Room room = aiComponent.Root().GetComponent<Room>();
            //room.Input.V = TSVector2.up;

            await ETTask.CompletedTask;
        }
    }
}