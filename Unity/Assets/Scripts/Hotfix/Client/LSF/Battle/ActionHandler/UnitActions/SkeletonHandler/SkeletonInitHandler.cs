using System;
using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET.Client
{
    [FriendOf(typeof(ActionComponent))]
    public class SkeletonInitHandler : AActionHandler
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