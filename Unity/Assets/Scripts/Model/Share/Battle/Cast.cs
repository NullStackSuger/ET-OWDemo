namespace ET
{
    [ChildOf(typeof(CastComponent))]
    public class Cast : LSEntity, IAwake<int>, IAwake<int, long>, IDestroy
    {
        public int ConfigId { get; set; }

        public EntityRef<LSUnit> Unit;
    }
}