using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFOperaComponent))]
    [FriendOf(typeof(LSFOperaComponent))]
    [FriendOf(typeof(Room))]
    public static partial class LSFOperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSFOperaComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this LSFOperaComponent self)
        {
            KeyCode key = KeyCode.None;

            TSVector2 v = new();
            if (Input.GetKey(KeyCode.W))
            {
                v.y += 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                v.x -= 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                v.y -= 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                v.x += 1;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                key = KeyCode.Space;
            }

            // 不能在这里检测CD
            // 这个是Update, 而LSFUpdateComponent 是LSUpdate
            // 很可能当满足Room.Update时 输入被覆盖掉了
            if (Input.GetKey(KeyCode.Q))
            {
                key = KeyCode.Q;
            }

            Room room = self.GetParent<Room>();
            room.Input.V = v.normalized;
            room.Input.Button = (int)key;
        }
    }
}