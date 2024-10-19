namespace ET.Client
{
    [FriendOf(typeof(ActionComponent))]
    public class SkeletonCastEHandler : AActionHandler
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