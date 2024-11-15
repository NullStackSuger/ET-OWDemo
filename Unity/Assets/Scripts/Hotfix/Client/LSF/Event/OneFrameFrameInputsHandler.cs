namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class OneFrameFrameInputsHandler : MessageHandler<Scene, OneFrameInputs>
    {
        protected override async ETTask Run(Scene entity, OneFrameInputs input)
        {
            // 这里只负责记录输入
            
            using var _ = input; // 方法结束时回收消息
            Room room = entity.GetComponent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;

            if (frameBuffer == null)
            {
                return;
            }
            
            OneFrameInputs authorityInput = frameBuffer.FrameInputs(input.Frame);
            if (input != authorityInput)
            {
                input.CopyTo(authorityInput);
            }
            
            room.Record(input);
            
            await ETTask.CompletedTask;
        }
    }
}