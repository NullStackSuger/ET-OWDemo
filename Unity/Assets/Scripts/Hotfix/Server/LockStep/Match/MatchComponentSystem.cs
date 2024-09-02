using System;
using System.Collections.Generic;

namespace ET.Server
{

    [FriendOf(typeof(MatchComponent))]
    public static partial class MatchComponentSystem
    {
        public static async ETTask Match(this MatchComponent self, long playerId)
        {
            // 防止一个人点2次算2个玩家
            if (self.WaitMatchPlayers.Contains(playerId))
            {
                Log.Warning($"{playerId}重复匹配");
                return;
            }

            self.WaitMatchPlayers.Add(playerId);

            if (self.WaitMatchPlayers.Count < LSFConfig.MatchCount)
            {
                Log.Warning($"等待匹配({self.WaitMatchPlayers.Count}/{LSFConfig.MatchCount})");
                RobotManagerComponent robotManagerComponent =
                        self.Root().GetComponent<RobotManagerComponent>() ?? self.Root().AddComponent<RobotManagerComponent>();
                await robotManagerComponent.NewRobot($"Robot{self.WaitMatchPlayers.Count}");
                return;
            }
            
            // 申请一个房间
            StartSceneConfig startSceneConfig = RandomGenerator.RandomArray(StartSceneConfigCategory.Instance.Maps);
            Match2Map_GetRoom match2MapGetRoom = Match2Map_GetRoom.Create();
            foreach (long id in self.WaitMatchPlayers)
            {
                match2MapGetRoom.PlayerIds.Add(id);
            }
            
            self.WaitMatchPlayers.Clear();

            Scene root = self.Root();
            Map2Match_GetRoom map2MatchGetRoom = await root.GetComponent<MessageSender>().Call(
                startSceneConfig.ActorId, match2MapGetRoom) as Map2Match_GetRoom;

            Match2G_NotifyMatchSuccess match2GNotifyMatchSuccess = Match2G_NotifyMatchSuccess.Create();
            match2GNotifyMatchSuccess.ActorId = map2MatchGetRoom.ActorId;
            MessageLocationSenderComponent messageLocationSenderComponent = root.GetComponent<MessageLocationSenderComponent>();
            
            foreach (long id in match2MapGetRoom.PlayerIds) // 这里发送消息线程不会修改PlayerInfo，所以可以直接使用
            {
                messageLocationSenderComponent.Get(LocationType.Player).Send(id, match2GNotifyMatchSuccess);
                // 等待进入房间的确认消息，如果超时要通知所有玩家退出房间，重新匹配
            }
        }
    }

}