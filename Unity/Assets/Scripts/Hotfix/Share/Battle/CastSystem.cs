namespace ET
{
    [EntitySystemOf(typeof(Cast))]
    [FriendOf(typeof(Cast))]
    public static partial class CastSystem
    {
        [EntitySystem]
        private static void Awake(this Cast self, int configId, LSUnit unit)
        {
            CastConfig config = CastConfigCategory.Instance.Get(configId);
            self.ConfigId = configId;
            self.Unit = unit;

            self.AddComponent<ActionComponent, int>(config.ActionGroup);

            LSUnit player = self.GetParent<CastComponent>().GetParent<LSUnit>();
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitUseCast() { Unit = player, Cast = self });
        }
    }
}