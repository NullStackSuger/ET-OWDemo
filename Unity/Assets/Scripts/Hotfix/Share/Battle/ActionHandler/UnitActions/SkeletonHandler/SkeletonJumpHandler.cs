using BulletSharp;
using BulletSharp.Math;

namespace ET{
    [FriendOf(typeof(CheckOnGroundComponent))]
    public class SkeletonJumpHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;

            if (input.Jump == false) return false;

            CheckOnGroundComponent checkOnGroundComponent = unit.GetComponent<CheckOnGroundComponent>();
            if (checkOnGroundComponent == null || !checkOnGroundComponent.OnGround) return false;

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            RigidBody body = unit.GetComponent<B3CollisionComponent>().Collision as RigidBody;
            float speed = 20;
            body.LinearVelocity = Vector3.UnitY * speed;
        }
    }
}