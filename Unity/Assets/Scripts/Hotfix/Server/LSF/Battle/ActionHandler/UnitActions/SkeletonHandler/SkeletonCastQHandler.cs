namespace ET.Server
{
    [FriendOf(typeof(ActionComponent))]
    [FriendOf(typeof(Cast))]
    public class SkeletonCastQHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;

            if (input.Button != 113) return false;

            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();

            // 检查技能间隔
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();
            if (!timerComponent.CheckTimeOut((long)actionComponent.Args["QRate"], true)) return false;

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            // 释放技能
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1012);
            
            /*CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1011);*/

            // 传送门
            /*CastComponent castComponent = unit.GetComponent<CastComponent>();
            LSUnit portalA = castComponent.Creat(1005).Unit;
            LSUnit portalB = castComponent.Creat(1006).Unit;
            portalA.AddComponent<PortalComponent, TSVector>(portalB.Position);
            portalB.AddComponent<PortalComponent, TSVector>(portalA.Position);*/

            /*CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1004);*/

            // 需要在Check里检查技能是否还存在
            /*CastComponent castComponent = unit.GetComponent<CastComponent>();
            Cast cast = castComponent.Creat(1009);
            actionComponent.Args.Add("ShieldId", castComponent.Id);*/
        }
    }
}