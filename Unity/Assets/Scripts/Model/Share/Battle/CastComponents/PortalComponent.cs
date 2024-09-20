using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class PortalComponent : LSEntity, IAwake<TSVector>
    {
        public TSVector Other;
    }
}