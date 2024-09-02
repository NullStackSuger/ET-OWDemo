//可以引用client代码, 然后在服务器里面启动多个robot

namespace ET.Client
{
    [Invoke((long)SceneType.Robot)]
    public class FiberInit_Robot: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.SceneType = SceneType.LockStepFrame;
            
            root.AddComponent<GlobalComponent>();
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<ObjectWait>();
            
            root.AddComponent<PlayerComponent>();
            root.AddComponent<CurrentScenesComponent>();
            
            await EventSystem.Instance.PublishAsync(root, new AfterCreatRobot());

            await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish());
            
            await LoginHelper.Login(root, root.Name, root.Name);
            
            await EnterMapHelper.Match(root.Fiber);
            
            root.AddComponent<AIComponent, int>(3);
        }
    }
}