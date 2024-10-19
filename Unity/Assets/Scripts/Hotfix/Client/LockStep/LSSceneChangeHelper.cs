using System.Collections.Generic;

namespace ET.Client
{

    public static partial class LSSceneChangeHelper
    {
        // 切场景
        public static async ETTask SceneChangeTo(Scene root, string sceneName, long sceneInstanceId)
        {
            long playerId = root.GetComponent<PlayerComponent>().MyId;
            
            Room room = root.AddComponentWithId<Room, string>(sceneInstanceId, sceneName);
            
            // 等待表现层订阅的事件完成
            await EventSystem.Instance.PublishAsync(root, new LSFSceneChange());
            
            root.GetComponent<ClientSenderComponent>().Send(C2Room_ChangeSceneFinish.Create());
            // 等待Room2C_EnterMap消息
            WaitType.Wait_Room2C_Start waitRoom2CStart = await root.GetComponent<ObjectWait>().Wait<WaitType.Wait_Room2C_Start>();
            
            room.Init(playerId, waitRoom2CStart.Message.UnitInfo, waitRoom2CStart.Message.StartTime);
            room.AddComponent<LSFUpdateComponent>();
            
            // 这个事件中可以订阅取消loading
            await EventSystem.Instance.PublishAsync(root, new LSFSceneInit());
        }
        
        // 回放
        public static async ETTask SceneChangeToReplay(Scene root, Replay replay)
        {
            root.RemoveComponent<Room>();
            long playerId = root.GetComponent<PlayerComponent>().MyId;
            
            Room room = root.AddComponent<Room, string>("Map1");
            room.Replay = replay;
            room.Init(playerId, replay.UnitInfos, TimeInfo.Instance.ServerFrameTime());

            await EventSystem.Instance.PublishAsync(root, new LSFSceneChange());

            room.AddComponent<ReplayUpdateComponent>();
            
            await EventSystem.Instance.PublishAsync(root, new LSFSceneInit());
        }
        
        // 重连
        public static async ETTask SceneChangeToReconnect(Scene root, G2C_Reconnect message, string sceneName)
        {
            root.RemoveComponent<Room>();
            long playerId = root.GetComponent<PlayerComponent>().MyId;

            Room room = root.AddComponent<Room, string>(sceneName);
            
            // 等待表现层订阅的事件完成
            await EventSystem.Instance.PublishAsync(root, new LSFSceneChange());
            
            room.Init(playerId, message.UnitInfos, message.StartTime, message.Frame);
            
            room.AddComponent<LSFUpdateComponent>();

            await EventSystem.Instance.PublishAsync(root, new LSFSceneInit());
        }
    }
}