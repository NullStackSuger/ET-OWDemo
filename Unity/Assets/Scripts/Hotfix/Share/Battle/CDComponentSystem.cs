namespace ET
{
    [EntitySystemOf(typeof(CDComponent))]
    [LSEntitySystemOf(typeof(CDComponent))]
    [FriendOfAttribute(typeof(ET.CDComponent))]
    public static partial class CDComponentSystem
    {
        [LSEntitySystem]
        private static void Awake(this ET.CDComponent self, long cd)
        {
            self.LastRecordTime = TimeInfo.Instance.ServerNow();
            self.CD = cd;
        }
        [LSEntitySystem]
        private static void LSUpdate(this ET.CDComponent self)
        {
            if (self.CD + self.LastRecordTime > TimeInfo.Instance.ServerNow())
            {
                Entity entity = self.Parent;
                entity.RemoveComponent<CDComponent>();
            }
        }
        [LSEntitySystem]
        private static void Destroy(this ET.CDComponent self)
        {

        }
    }
}