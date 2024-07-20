using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{

    [EntitySystemOf(typeof(LSF_UIRoomComponent))]
    [FriendOf(typeof(LSF_UIRoomComponent))]
    public static partial class LSF_UIRoomComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSF_UIRoomComponent self)
        {
            
        }

        [EntitySystem]
        private static void Update(this LSF_UIRoomComponent self)
        {
            
        }
    }
}