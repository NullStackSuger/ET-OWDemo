namespace ET
{
    [FriendOf(typeof(ActionComponent))]
    public class SkeletonCastEHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;

            if (input.Button != 101) return false;

            // 检查技能间隔
            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            if (!timerComponent.CheckTimeOut((long)actionComponent.Args["ERate"], true)) return false;

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            // 释放技能
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            /*CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1002);*/
            
            // 需要在Check里检查技能是否还存在
            CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1009);
            //actionComponent.Args.Add("ShieldId", castComponent.Id);
            
            /*CastComponent castComponent = unit.GetComponent<CastComponent>();
            Cast cast = castComponent.Creat(1010);*/
        }
    }
}