using BulletSharp;
using TrueSync;
using Vector3 = BulletSharp.Math.Vector3;

namespace ET.Client
{
    [FriendOf(typeof(Cast))]
    public class FireBallInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
        }
    }

    [FriendOf(typeof(Cast))]
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