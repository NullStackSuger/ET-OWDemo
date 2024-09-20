namespace ET
{
    [EntitySystemOf(typeof(PortalComponent))]
    [FriendOf(typeof(PortalComponent))]
    public static partial class PortalComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.PortalComponent self, TrueSync.TSVector other)
        {
            self.Other = other;
        }
    }
}