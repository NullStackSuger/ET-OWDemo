using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 记录切换Scene完成的玩家信息
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class WaitChangeSceneComponent : Entity, IAwake
    {
        public List<long> PlayerIds = new();
    }
}