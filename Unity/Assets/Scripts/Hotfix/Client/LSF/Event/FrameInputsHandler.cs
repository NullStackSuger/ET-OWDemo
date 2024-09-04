using System;
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

            if (frameBuffer == null)
            {
                Log.Warning($"{entity.Name}未初始化");
                return;
            }

            // 这里防止因为些奇怪的操作导致客户端和服务端包对不上
            // 我这里好像因为加了个人机, 电脑卡了, 导致服务端比客户端快了1帧
            while (room.AuthorityFrame < input.Frame)
            {
                ++room.AuthorityFrame;
            }

            // TODO KCP为什么发包顺序不对?
            // 防止因为发包的顺序不对
            if (input.Frame < room.AuthorityFrame)
            {
                OneFrameInputs authorityFrame = frameBuffer.FrameInputs(room.AuthorityFrame);
                input.CopyTo(authorityFrame);
            }
            
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
                OneFrameInputs authorityFrame = frameBuffer.FrameInputs(room.AuthorityFrame);
                // 对比失败
                if (input != authorityFrame)
                {
                    // 每次print回滚会打印2次是正常的, 因为我们没有实际执行回滚代码
                    Log.Warning($"回滚 {entity.Name}:{room.AuthorityFrame}\r\n" +
                        $"Client: {authorityFrame.Inputs.Values.ToArray().ToJson()}\r\n" +
                        $"ClientLast: {frameBuffer.FrameInputs(room.AuthorityFrame - 1).Inputs.Values.ToArray().ToJson()}\r\n" +
                        $"Server: {input.Inputs.Values.ToArray().ToJson()}");
                    input.CopyTo(authorityFrame);
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