using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class LSF_UIRoomComponent : Entity, IAwake, IUpdate
    {
        public Text CurrentBulletCount;
        public Text MaxBulletCount;

        public Text CurrentHp;
        public Text MaxHp;
    }
}