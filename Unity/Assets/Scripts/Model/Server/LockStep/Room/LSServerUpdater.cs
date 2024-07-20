using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(ET.Room))]
    public class LSServerUpdater: Entity, IAwake, IUpdate
    {
    }
}