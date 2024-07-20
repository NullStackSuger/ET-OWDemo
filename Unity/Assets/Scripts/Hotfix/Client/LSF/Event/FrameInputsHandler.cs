using System.Linq;
using MongoDB.Bson;

namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class FrameInputsHandler : MessageHandler<Scene, OneFrameInputs>
    {
        protected override async ETTask Run(Scene entity, OneFrameInputs input)
        {
            using var _ = input; // 方法结束时回收消息
            Room room = entity.GetComponent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            
            if (frameBuffer == null) return;
            
            ++room.AuthorityFrame;
            
            // 服务端返回的消息比预测的还早
            // 就设置Buffer里为服务器的消息
            if (room.AuthorityFrame > room.PredictionFrame)
            {
                OneFrameInputs authorityFrame = frameBuffer.FrameInputs(room.AuthorityFrame);
                input.CopyTo(authorityFrame);
            }
            else
            {
                // 服务端消息和客户端权威帧比较
                OneFrameInputs predictionInput = frameBuffer.FrameInputs(room.AuthorityFrame);
                // 对比失败
                if (input != predictionInput)
                {
                    //Log.Warning($"Client Rollback {room.AuthorityFrame} | Client:{predictionInput.ToJson()} | Server:{input.ToJson()} | ClientLast:{frameBuffer.FrameInputs(room.AuthorityFrame - 1).ToJson()}");
                    input.CopyTo(predictionInput);
                    
                    Log.Warning($"对比失败 开启回滚");
                    //room.Rollback();
                }
                // 对比成功
                else
                {
                    //room.SendHash(room.AuthorityFrame);
                }
            }
            
            // 对比失败会调用Rollback更正结果, 3钟情况都可以记录
            room.Record(room.AuthorityFrame);
            
            await ETTask.CompletedTask;
        }
    }
}