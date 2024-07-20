/*using System;

namespace ET.Client
{
    // Client.Receive
    [MessageHandler(SceneType.LockStep)]
    public class OneFrameInputsHandler: MessageHandler<Scene, OneFrameInputs>
    {
        protected override async ETTask Run(Scene root, OneFrameInputs message)
        {
            using var _ = message ; // 方法结束时回收消息
            ET.Room room = root.GetComponent<ET.Room>();
            
            Log.Debug($"OneFrameInputs: {room.AuthorityFrame + 1} {message.ToJson()}");
                        
            FrameBuffer frameBuffer = room.FrameBuffer;

            ++room.AuthorityFrame;
            
            Log.Warning($"Client AuFrame:{room.AuthorityFrame} | Client PreFrame {room.PredictionFrame}");
            
            // 服务端返回的消息比预测的还早
            // 就设置Buffer里为服务器的消息
            if (room.AuthorityFrame > room.PredictionFrame)
            {
                OneFrameInputs authorityFrame = frameBuffer.FrameInputs(room.AuthorityFrame);
                message.CopyTo(authorityFrame);
            }
            else
            {
                // 服务端返回来的消息，跟预测消息对比
                OneFrameInputs predictionInput = frameBuffer.FrameInputs(room.AuthorityFrame);
                
                // Check
                // 对比失败有两种可能，
                // 1是别人的输入预测失败，这种很正常，
                // 2 自己的输入对比失败，这种情况是自己发送的消息比服务器晚到了，服务器使用了你的上一次输入
                // 回滚重新预测的时候，自己的输入不用变化
                if (message != predictionInput)
                {
                    Log.Debug($"frame diff: {predictionInput} {message}");
                    message.CopyTo(predictionInput);
                    // 回滚到frameBuffer.AuthorityFrame
                    Log.Debug($"roll back start {room.AuthorityFrame}");
                    LSClientHelper.Rollback(room, room.AuthorityFrame);
                    Log.Debug($"roll back finish {room.AuthorityFrame}");
                }
                else // 对比成功
                {
                    room.Record(room.AuthorityFrame);
                    room.SendHash(room.AuthorityFrame);
                }
            }
            
            await ETTask.CompletedTask;
        }
    }
}*/