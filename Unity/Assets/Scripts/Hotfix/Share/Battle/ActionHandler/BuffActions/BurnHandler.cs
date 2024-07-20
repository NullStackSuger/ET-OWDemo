namespace ET
{

    public class BurnHandler : AActionHandler

    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            long sec = TimeInfo.Instance.ClientNow() / 1000 % 15;
            if (sec < 10)
            {
                return true;
            }
            return false;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSWorld world = actionComponent.IScene as LSWorld;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            Log.Warning($"Burning");
        }
    }
}