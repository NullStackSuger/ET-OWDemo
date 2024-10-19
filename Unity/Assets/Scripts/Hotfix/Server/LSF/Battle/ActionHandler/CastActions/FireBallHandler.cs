using BulletSharp;
using TrueSync;
using Vector3 = BulletSharp.Math.Vector3;

namespace ET.Server
{
    [FriendOf(typeof(Cast))]
    [FriendOf(typeof(B3CollisionComponent))]
    [FriendOf(typeof(B3WorldComponent))]
    public class FireBallInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit castUnit = actionComponent.GetParent<LSUnit>();

            B3CollisionComponent collision = castUnit.GetComponent<B3CollisionComponent>();
            
            TSMatrix matrix = TSMath.RotationMatrix(castUnit.HeadRotation, castUnit.Rotation);
            TSVector offset = matrix * TSVector.forward * 1000;
            RigidBody body = collision.Collision as RigidBody;
            body.Gravity = Vector3.Zero;
            body.ApplyCentralForce(offset.ToBullet());
        }
    }

    [FriendOf(typeof(Cast))]
    [FriendOf(typeof(B3CollisionComponent))]
    public class FireBallHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
        }
    }
}