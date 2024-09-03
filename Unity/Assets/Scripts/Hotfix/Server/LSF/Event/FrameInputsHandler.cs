using System.Linq;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    public class FrameInputsHandler : MessageHandler<Scene, FrameMessage>
    {
        protected override async ETTask Run(Scene entity, FrameMessage message)
        {
            using FrameMessage _ = message;
            
            // 获取第一个Room
            Room room = entity.GetComponent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            
            // 判断每秒钟执行一次
            if (message.Frame % (1000 / LSConstValue.UpdateInterval) == 0)
            {
                long nowFrameTime = room.FixedTimeCounter.FrameTime(message.Frame);
                int diffTime = (int)(nowFrameTime - TimeInfo.Instance.ServerFrameTime());
                
                Room2C_AdjustUpdateTime room2CAdjustUpdateTime = Room2C_AdjustUpdateTime.Create();
                room2CAdjustUpdateTime.DiffTime = diffTime;
                room.Send(message.PlayerId, room2CAdjustUpdateTime);
            }
            
            if (message.Frame < room.AuthorityFrame)  // 小于AuthorityFrame，丢弃
            {
                Log.Warning($"FrameMessage < AuthorityFrame discard: {message.Frame} {room.AuthorityWorld}");
                return;
            }

            if (message.Frame > room.AuthorityFrame + 10)  // 大于AuthorityFrame + 10，丢弃
            {
                Log.Warning($"FrameMessage > AuthorityFrame + 10 discard: {message}");
                return;
            }
            
            OneFrameInputs oneFrameInputs = frameBuffer.FrameInputs(message.Frame);
            if (oneFrameInputs == null)
            {
                Log.Error($"FrameMessageHandler get frame is null: {message.Frame}, max frame: {frameBuffer.MaxFrame}");
                return;
            }
            
            //Log.Warning($"收到{message.PlayerId}第{message.Frame}帧输入{message.Input.V}");
            oneFrameInputs.Inputs[message.PlayerId] = message.Input;
            
            await ETTask.CompletedTask;
        }
    }
}