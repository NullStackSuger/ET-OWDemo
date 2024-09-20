namespace ET
{
    [FriendOf(typeof(ActionComponent))]
    [FriendOf(typeof(Cast))]
    public class HpStickInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSWorld world = actionComponent.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            long id = timerComponent.AddTimer(1000);
            actionComponent.Args.Add("Rate", id);
        }
    }
    
    [FriendOf(typeof(ActionComponent))]
    [FriendOf(typeof(Cast))]
    public class HpStickHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSWorld world = actionComponent.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            long rateTimerId = (long)actionComponent.Args["Rate"];
            return timerComponent.CheckTimeOut(rateTimerId, true);
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit castUnit = actionComponent.GetParent<LSUnit>();
            castUnit.AddComponent<B3CollisionComponent, int>(6);
        }
    }
}