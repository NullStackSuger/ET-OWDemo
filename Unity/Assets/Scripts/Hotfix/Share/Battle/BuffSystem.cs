namespace ET
{
    [EntitySystemOf(typeof(Buff))]
    public static partial class BuffSystem
    {
        [EntitySystem]
        private static void Awake(this Buff self, int configId)
        {
            LSUnit player = self.GetParent<BuffComponent>().GetParent<LSUnit>();
            BuffConfig config = BuffConfigCategory.Instance.Get(configId);
            self.ConfigId = configId;
            
            // 添加行为机
            self.AddComponent<ActionComponent, int>(config.ActionGroup);
            
            // 发布事件: 受到Buff
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitUseBuff() { Unit = player, Buff = self });
        }

        [EntitySystem]
        private static void Destroy(this Buff self)
        {
            // 发布事件: 销毁技能
            LSUnit player = self.GetParent<BuffComponent>().GetParent<LSUnit>();
            EventSystem.Instance.Publish(self.IScene as LSWorld, new UnitRemoveBuff() { Unit = player, Buff = self });
        }
    }
}