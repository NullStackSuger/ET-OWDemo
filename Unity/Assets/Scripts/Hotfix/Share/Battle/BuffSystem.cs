namespace ET
{
    [EntitySystemOf(typeof(Buff))]
    public static partial class BuffSystem
    {
        [EntitySystem]
        private static void Awake(this Buff self, int configId)
        {
            BuffConfig config = BuffConfigCategory.Instance.Get(configId);
            self.ConfigId = configId;
            
            self.AddComponent<ActionComponent, int>(config.ActionGroup);
        }
    }
}