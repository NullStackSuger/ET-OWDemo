using System.Reflection;
using BulletSharp.Math;
using TrueSync;

namespace ET
{
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
            Cast cast = actionComponent.GetParent<Cast>();
            LSUnit castUnit = cast.Unit;
            
            B3CollisionComponent collision = castUnit.GetComponent<B3CollisionComponent>();
            collision.Body.ApplyForce(new Vector3(0, 0, 700), Vector3.Zero);

            // 范围检测
        }
    }
}