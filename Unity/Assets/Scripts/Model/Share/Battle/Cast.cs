
namespace ET
{
    [ChildOf(typeof(CastComponent))]
    public class Cast : LSEntity, IAwake<int>, IDestroy
    {
        public int ConfigId { get; set; }

        public EntityRef<LSUnit> Unit;
    }
}