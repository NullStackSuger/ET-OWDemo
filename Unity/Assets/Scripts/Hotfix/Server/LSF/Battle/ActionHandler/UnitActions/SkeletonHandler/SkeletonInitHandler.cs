using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET.Server
{
    [FriendOf(typeof(ActionComponent))]
    [FriendOf(typeof(B3WorldComponent))]
    public class SkeletonInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();

            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            LSFTimerComponent timerComponent = room.GetComponent<LSFTimerComponent>();

            long id = timerComponent.AddTimer(dataModifierComponent.Get(DataModifierType.ECD));
            actionComponent.Args.Add("ERate", id);

            id = timerComponent.AddTimer(dataModifierComponent.Get(DataModifierType.QCD));
            actionComponent.Args.Add("QRate", id);

            id = timerComponent.AddTimer(dataModifierComponent.Get(DataModifierType.CCD));
            actionComponent.Args.Add("CRate", id);

            // TODO 测试破坏球
            DynamicsWorld pyWorld = room.AuthorityWorld.GetComponent<B3WorldComponent>().World;
            RigidBody body = unit.GetComponent<B3CollisionComponent>().Collision as RigidBody;
            Point2PointConstraint constraint = new Point2PointConstraint(body, new Vector3(0, 10, 10));
            pyWorld.AddConstraint(constraint);
        }
    }
}