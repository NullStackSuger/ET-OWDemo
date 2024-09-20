namespace ET
{
    [FriendOf(typeof(Cast))]
    [FriendOf(typeof(ActionComponent))]
    public class FortInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();

            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();

            dataModifierComponent.Add(new Default_Hp_FinalMaxModifier() { Value = 20 });
            dataModifierComponent.Add(new Default_Hp_FinalMinModifier() { Value = 0 });
            dataModifierComponent.Add(new Default_Hp_ConstantModifier() { Value = 20 });

            dataModifierComponent.Add(new Default_FortNumeric_ConstantModifier() { Value = 5 });

            dataModifierComponent.Add(new Default_FortRate_ConstantModifier() { Value = 2000 });

            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            long id = timerComponent.AddTimer(dataModifierComponent.Get(DataModifierType.FortRate));
            actionComponent.Args.Add("Rate", id);
        }
    }
    [FriendOf(typeof(Cast))]
    [FriendOf(typeof(ActionComponent))]
    public class FortHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            if (dataModifierComponent.Get(DataModifierType.Hp) <= 0)
            {
                LSUnit owner = unit.Owner;
                owner.GetComponent<CastComponent>().Remove(unit.Id);
                return false;
            }
            
            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            if (!timerComponent.CheckTimeOut((long)actionComponent.Args["Rate"], true)) return false;
            
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            
            CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1008);
        }
    }
}