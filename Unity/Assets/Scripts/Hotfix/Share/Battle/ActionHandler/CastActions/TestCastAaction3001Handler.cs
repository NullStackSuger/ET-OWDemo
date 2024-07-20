namespace ET
{

    public class TestCastAction3001Handler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            Log.Warning($"Client {actionComponent.Parent.GetType()} {nameof(TestCastAction3001Handler)} Check");
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            Log.Warning($"Client {actionComponent.Parent.GetType()} {nameof(TestCastAction3001Handler)} Execute");
        }
    }
}