using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(Room))]
    public static partial class RoomSystem
    {
        [EntitySystem]
        private static void Awake(this Room self, string name)
        {
            self.Name = name;
        }
    }
}