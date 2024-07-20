namespace ET
{
    [ChildOf(typeof(BuffComponent))]
    public class Buff : LSEntity, IAwake<int>
    {
        public int ConfigId { get; set; }
    }
}