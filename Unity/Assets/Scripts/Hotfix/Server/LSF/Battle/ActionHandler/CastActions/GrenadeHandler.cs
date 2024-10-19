using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET.Server
{
    [FriendOf(typeof(Cast))]
    public class GrenadeInitHandler : AActionHandler
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
    public class GrenadeHandler : AActionHandler
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