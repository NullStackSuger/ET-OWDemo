using BulletSharp;
using BulletSharp.Math;

namespace ET
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
            Cast cast = actionComponent.GetParent<Cast>();
            LSUnit castUnit = cast.Unit;

            B3CollisionComponent collision = castUnit.GetComponent<B3CollisionComponent>();
            //collision.Body.ApplyForce(castUnit.Forward.ToVector() * 1000, Vector3.Zero);
            collision.Body.WorldTransform += Matrix.Translation(0, 0, 10);
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