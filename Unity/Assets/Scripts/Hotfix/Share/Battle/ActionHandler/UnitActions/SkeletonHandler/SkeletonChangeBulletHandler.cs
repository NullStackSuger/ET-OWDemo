namespace ET{

    public class SkeletonChangeBulletHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;
            if (input.Button != 114) return false;

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();

            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            var values = dataModifierComponent.GetValues(DataModifierType.BulletCount);
            dataModifierComponent.Add(new Default_BulletCount_ConstantModifier() { Value = values.Item2 - values.Item1 }, true);
        }
    }
}