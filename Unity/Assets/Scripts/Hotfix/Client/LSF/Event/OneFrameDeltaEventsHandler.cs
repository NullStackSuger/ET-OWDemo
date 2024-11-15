using MongoDB.Bson;

namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class OneFrameDeltaEventsHandler : MessageHandler<Scene, OneFrameDeltaEvents>
    {
        protected override async ETTask Run(Scene scene, OneFrameDeltaEvents message)
        {
            // 这里负责回滚
            
            using var _ = message; // 方法结束时回收消息
            Room room = scene.GetComponent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            
            if (frameBuffer == null)
            {
                return;
            }
            
            ++room.AuthorityFrame;
            
            OneFrameDeltaEvents authorityEvent = frameBuffer.DeltaEvents(room.AuthorityFrame);

            // 如果输入不一致,
            // 事件一致时, 没事,
            // 事件不一致时, 回滚
            bool needRollback = room.AuthorityFrame <= room.PredictionFrame && message != authorityEvent;
            
            // 即使对比成功也可能有些很小的误差, 还是用服务端的最准
            message.CopyTo(authorityEvent);

            if (needRollback)
            {
                Log.Warning($"回滚{room.AuthorityFrame}");
                room.Rollback();
            }

            if (!room.IsReplay)
            {
                // 防止这一帧没东西, 导致少Add一个
                room.Record(room.AuthorityFrame, "", null);
                room.Record(room.AuthorityFrame);
            }
            
            await ETTask.CompletedTask;
        }
    }
}