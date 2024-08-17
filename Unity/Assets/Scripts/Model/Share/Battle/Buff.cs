namespace ET
{
    [ChildOf(typeof(BuffComponent))]
    public class Buff : LSEntity, IAwake<int>, IDestroy
    {
        public int ConfigId { get; set; }
    }
}