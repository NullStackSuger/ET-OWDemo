namespace ET.Client
{
    [EntitySystemOf(typeof(RobotInputFinishComponent))]
    public static partial class RobotInputFinishComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RobotInputFinishComponent self)
        {

        }
        [EntitySystem]
        private static void Update(this RobotInputFinishComponent self)
        {
            // 发送消息
            Scene root = self.Root();
            EventSystem.Instance.Publish(root, new InputFinish());
        }
    }
}