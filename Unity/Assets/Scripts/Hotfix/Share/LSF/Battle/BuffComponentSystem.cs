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
            string name = BuffConfigCategory.Instance.Get(configId).Name;
            self.Buffs.Add(name, buff);
            return buff;
        }

        public static Buff Creat(this BuffComponent self, int configId, long id)
        {
            Buff buff = self.AddChildWithId<Buff, int>(id, configId);
            string name = BuffConfigCategory.Instance.Get(configId).Name;
            self.Buffs.Add(name, buff);
            return buff;
        }

        public static void Remove(this BuffComponent self, Buff buff)
        {
            string name = BuffConfigCategory.Instance.Get(buff.ConfigId).Name;
            self.Buffs.Remove(name);
            self.RemoveChild(buff.Id);
        }

        public static void Remove(this BuffComponent self, long buffId)
        {
            Buff buff = self.GetChild<Buff>(buffId);
            if (buff == null)  Log.Error($"{self.IScene.SceneType}: 未找到{buffId}Buff");
            self.Remove(buff);
        }

        public static bool Has(this BuffComponent self, string name)
        {
            return self.Buffs.ContainsKey(name);
        }
    }
}