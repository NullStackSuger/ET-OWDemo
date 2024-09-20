using BulletSharp;

namespace ET
{
    [EntitySystemOf(typeof(LSUnitComponent))]
    [LSEntitySystemOf(typeof(LSUnitComponent))]
    [FriendOf(typeof(LSUnitComponent))]
    public static partial class LSUnitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSUnitComponent self)
        {

        }

        [LSEntitySystem]
        private static void LSUpdate(this LSUnitComponent self)
        {
            while (self.WaitToRemove.TryDequeue(out long id))
            {
                self.RemoveChild(id);
            }
        }

        public static LSUnit Creat(this LSUnitComponent self)
        {
            LSUnit unit = self.AddChild<LSUnit>();
            return unit;
        }

        public static LSUnit Creat(this LSUnitComponent self, long id)
        {
            LSUnit unit = self.AddChildWithId<LSUnit>(id);
            return unit;
        }

        public static LSUnit Creat(this LSUnitComponent self, LockStepUnitInfo playerInfo, string tag)
        {
            LSUnit unit = self.AddChildWithId<LSUnit>(playerInfo.PlayerId);

            unit.Position = playerInfo.Position;
            unit.Rotation = playerInfo.Rotation;
            unit.AddComponent<ActionComponent, int>(playerInfo.ActionGroup);
            unit.AddComponent<BuffComponent>();
            unit.AddComponent<CastComponent>();
            unit.Tag = tag;

            return unit;
        }

        public static void WaitToRemove(this LSUnitComponent self, long id)
        {
            self.WaitToRemove.Enqueue(id);
        }
    }
}