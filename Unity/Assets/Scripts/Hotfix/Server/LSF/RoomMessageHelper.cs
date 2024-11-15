using System.Collections.Generic;

namespace ET.Server
{
    public static partial class RoomMessageHelper
    {
        public static void BroadCast(this ET.Room room, IMessage message)
        {
            // 广播的消息不能被池回收
            (message as MessageObject).IsFromPool = false;
            
            MessageLocationSenderComponent messageLocationSenderComponent = room.Root().GetComponent<MessageLocationSenderComponent>();
            foreach (var id in room.PlayerIds)
            {
                var type = messageLocationSenderComponent.Get(LocationType.GateSession);
                type.Send(id, message);
            }
        }

        public static void Send(this ET.Room room, long playerId, IMessage message)
        {
            (message as MessageObject).IsFromPool = false;
            MessageLocationSenderComponent messageLocationSenderComponent = room.Root().GetComponent<MessageLocationSenderComponent>();
            messageLocationSenderComponent.Get(LocationType.GateSession).Send(playerId, message);
        }

        public static void BroadCast(this ET.Room room, IMessage message, List<long> ids)
        {
            // 广播的消息不能被池回收
            (message as MessageObject).IsFromPool = false;
            
            MessageLocationSenderComponent messageLocationSenderComponent = room.Root().GetComponent<MessageLocationSenderComponent>();
            foreach (var id in ids)
            {
                var type = messageLocationSenderComponent.Get(LocationType.GateSession);
                type.Send(id, message);
            }
        }

        public static void BroadCast(this LSUnit self, IRoomMessage message)
        {
            // 广播的消息不能被池回收
            (message as MessageObject).IsFromPool = false;
            
            MessageLocationSenderComponent messageLocationSenderComponent = self.Root().GetComponent<MessageLocationSenderComponent>();
            AOIEntity aoiEntity = self.GetComponent<AOIEntity>();
            foreach (AOIEntity entity in aoiEntity.GetAOI())
            {
                var type = messageLocationSenderComponent.Get(LocationType.GateSession);
                type.Send(entity.GetParent<LSUnit>().Id, message);
            }
        }
    }
}