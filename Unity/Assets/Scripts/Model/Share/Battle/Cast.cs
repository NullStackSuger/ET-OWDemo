namespace ET
{
    [ChildOf(typeof(CastComponent))]
    public class Cast : LSEntity, IAwake<int, LSUnit>
    {
        public int ConfigId { get; set; }

        public EntityRef<LSUnit> Unit;
    }
}