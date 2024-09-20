namespace ET
{
    [FriendOf(typeof(ActionComponent))]
    public class SkeletonCastCHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;

            if (input.Button != 99) return false;
            
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();

            int needBulletCount = 10;
            // 检查子弹数
            long bulletCount = dataModifierComponent.Get(DataModifierType.BulletCount);
            if (bulletCount < needBulletCount) return false;

            // 检查技能间隔
            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            if (!timerComponent.CheckTimeOut((long)actionComponent.Args["CRate"], true)) return false;

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            // 释放技能
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1001);
            
            int needBulletCount = 10;
            // 减子弹数
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            dataModifierComponent.Add(new Default_BulletCount_ConstantModifier() { Value = -needBulletCount }, true);
        }
    }
}