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
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.CurrentBulletCount = rc.Get<GameObject>("Current Bullet Count").GetComponent<Text>();
            self.MaxBulletCount = rc.Get<GameObject>("Max Bullet Count").GetComponent<Text>();
            self.CurrentHp = rc.Get<GameObject>("Current Hp").GetComponent<Text>();
            self.MaxHp = rc.Get<GameObject>("Max Hp").GetComponent<Text>();
        }

        [EntitySystem]
        private static void Update(this LSF_UIRoomComponent self)
        {
            
        }
    }
}