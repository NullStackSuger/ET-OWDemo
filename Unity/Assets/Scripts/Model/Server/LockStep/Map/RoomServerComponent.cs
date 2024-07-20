using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(ET.Room))]
    public class RoomServerComponent: Entity, IAwake<List<long>>
    {
    }
}