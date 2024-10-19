using TrueSync;

namespace ET.Server
{
    public class EyepieceInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            
        }
    }
    [FriendOf(typeof(ActionComponent))]
    public class EyepieceHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            // 遍历LSUnitComponent, 查找20m以内的距离最近的敌方玩家
            LSUnit owner = actionComponent.GetParent<Buff>().GetParent<BuffComponent>().GetParent<LSUnit>();
            LSWorld world = actionComponent.IScene as LSWorld;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();

            LSUnit trigger = null;
            FP minDistance = FP.MaxValue;
            foreach (Entity entity in unitComponent.Children.Values)
            {
                if (entity is not LSUnit unit) continue;

                FP distance = TSMath.DistanceSquared(unit.Position, owner.Position);
                if (distance < 20) continue;

                if (Tag.IsFriend(owner.Tag, unit.Tag)) continue;

                if (distance >= minDistance) continue;

                trigger = unit;
                minDistance = distance;
            }

            actionComponent.Args["Trigger"] = trigger;

            return trigger != null;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit trigger = actionComponent.Args["Trigger"] as LSUnit;
            if (trigger == null) return;
            LSUnit owner = actionComponent.GetParent<Buff>().GetParent<BuffComponent>().GetParent<LSUnit>();
            TSVector vector = trigger.Position - owner.Position;
            vector.y = 0;
            owner.Rotation = TSMath.Acos(TSVector.Dot(TSVector.forward, vector.normalized)) * TSMath.Rad2Deg;

        }
    }
}