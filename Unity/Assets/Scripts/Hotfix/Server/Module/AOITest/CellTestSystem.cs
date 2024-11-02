namespace ET.Server
{
    [EntitySystemOf(typeof(CellTest))]
    [FriendOf(typeof(CellTest))]
    public static partial class CellTestSystem
    {
        [EntitySystem]
        private static void Awake(this CellTest self)
        {

        }
        [EntitySystem]
        private static void Destroy(this CellTest self)
        {
            self.Units.Clear();
        }
    }
}