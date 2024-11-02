using System.Collections.Generic;
using TrueSync;

namespace ET.Server
{
    [EntitySystemOf(typeof(AOIEntityTest))]
    [FriendOf(typeof(AOIEntityTest))]
    public static partial class AOIEntityTestSystem
    {
        [EntitySystem]
        private static void Awake(this AOIEntityTest self)
        {
        }
        [EntitySystem]
        private static void Destroy(this AOIEntityTest self)
        {
            
        }

        // 关注一个Entity
        public static void Add(this AOIEntityTest self, AOIEntityTest other)
        {
            // 发布一个UnitEnterSightRange事件, 并向客户端广播
        }

        // 取消关注一个Entity
        public static void Remove(this AOIEntityTest self, AOIEntityTest other)
        {
            // 发布一个UnitLeaveSightRange事件, 并向客户端广播
        }
    }
}