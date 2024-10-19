namespace ET.Client
{
    [FriendOf(typeof(ActionComponent))]
    [FriendOf(typeof(Cast))]
    public class SkeletonCastQHandler : AActionHandler
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