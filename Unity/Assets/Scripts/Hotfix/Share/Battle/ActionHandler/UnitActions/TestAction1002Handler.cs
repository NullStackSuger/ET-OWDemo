namespace ET.Server
{
    public class TestAction1002Handler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            Log.Warning($"{nameof(TestAction1002Handler)} Update");
        }
    }
}