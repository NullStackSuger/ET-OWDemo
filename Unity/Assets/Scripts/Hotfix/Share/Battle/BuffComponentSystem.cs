namespace ET
{
    [EntitySystemOf(typeof(BuffComponent))]
    [FriendOf(typeof(BuffComponent))]
    public static partial class BuffComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuffComponent self)
        {

        }

        public static Buff Creat(this BuffComponent self, int configId)
        {
            Buff buff = self.AddChild<Buff, int>(configId);
            self.Buffs.Add(buff);
            return buff;
        }

        public static void Remove(this BuffComponent self, Buff buff)
        {
            self.Buffs.Remove(buff);
            self.RemoveChild(buff.Id);
        }
    }
}