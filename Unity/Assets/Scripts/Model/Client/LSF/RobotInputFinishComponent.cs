namespace ET.Client
{
    [ComponentOf(typeof(AIComponent))] // 用于约束输入只能在AIComponent中设置
    public class RobotInputFinishComponent : Entity, IAwake, IUpdate
    {

    }
}