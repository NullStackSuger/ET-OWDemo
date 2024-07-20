namespace ET.Server
{
    public class TestAction1001Handler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            Log.Warning($"{nameof(TestAction1001Handler)} Update");
        }
    }
}