namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class G2C_ReconnectHandler: MessageHandler<Scene, G2C_Reconnect>
    {
        protected override async ETTask Run(Scene root, G2C_Reconnect message)
        {
            await LSSceneChangeHelper.SceneChangeToReconnect(root, message, "Map1");
            await ETTask.CompletedTask;
        }
    }
}