namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class UnPredictionMessageHandler : MessageHandler<Scene, UnPredictionMessage>
    {
        protected override async ETTask Run(Scene entity, UnPredictionMessage message)
        {
            using var _ = message; // 方法结束时回收消息
            
            Room room = entity.GetComponent<Room>();
            
            if (message.Frame <= room.PredictionFrame) return; // 如果发晚了就不会再改消息了, 防止同步出错
            
            FrameBuffer frameBuffer = room.FrameBuffer;
            OneFrameInputs frameInputs = frameBuffer.FrameInputs(message.Frame);
            
            frameInputs.Inputs.TryAdd(message.PlayerId, new LSInput());
            LSInput input = frameInputs.Inputs[message.PlayerId];
            LSInput.CopyUnPrediction(message, ref input);
            frameInputs.Inputs[message.PlayerId] = input;
            
            await ETTask.CompletedTask;
        }
    }
}