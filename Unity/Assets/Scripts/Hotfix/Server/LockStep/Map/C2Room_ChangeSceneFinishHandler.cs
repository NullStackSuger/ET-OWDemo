using System.Collections.Generic;
using TrueSync;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    [FriendOf(typeof(RoomServerComponent))]
    [FriendOfAttribute(typeof(ET.Server.WaitChangeSceneComponent))]
    public class C2Room_ChangeSceneFinishHandler : MessageHandler<Scene, C2Room_ChangeSceneFinish>
    {
        protected override async ETTask Run(Scene root, C2Room_ChangeSceneFinish message)
        {
            WaitChangeSceneComponent waitChangeSceneComponent =
                    root.GetComponent<WaitChangeSceneComponent>() ??
                    root.AddComponent<WaitChangeSceneComponent>();
            Room room = root.GetComponent<Room>();
            
            waitChangeSceneComponent.PlayerIds.Add(message.PlayerId);

            if (waitChangeSceneComponent.PlayerIds.Count < LSFConfig.MatchCount)
            {
                return;
            }
            
            await room.Fiber.Root.GetComponent<TimerComponent>().WaitAsync(1000);
            
            Room2C_Start room2CStart = Room2C_Start.Create();
            room2CStart.StartTime = TimeInfo.Instance.ServerFrameTime();
            foreach (long id in waitChangeSceneComponent.PlayerIds)
            {
                LockStepUnitInfo lockStepUnitInfo = LockStepUnitInfo.Create();
                lockStepUnitInfo.PlayerId = id;
                lockStepUnitInfo.Position = new TSVector(0, 3, 0);
                lockStepUnitInfo.Rotation = 0;
                lockStepUnitInfo.ActionGroup = 3;
                room2CStart.UnitInfo.Add(lockStepUnitInfo);
            }
            
            room.Init(room2CStart.UnitInfo, room2CStart.StartTime);
            room.AddComponent<LSFUpdateComponent>();
            
            room.BroadCast(room2CStart);
            
            root.RemoveComponent<WaitChangeSceneComponent>();

            await ETTask.CompletedTask;
        }
    }
}