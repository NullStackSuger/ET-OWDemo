namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class CheckOnGroundComponent : LSEntity, IAwake, ILSUpdate
    {
        public bool OnGround;
    }
}