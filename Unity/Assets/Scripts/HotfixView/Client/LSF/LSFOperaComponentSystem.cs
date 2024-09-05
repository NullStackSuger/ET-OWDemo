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
            var inputSystem = new SkeletonInputSystem();

            self.InputSystem = inputSystem;
            self.InputSystem.Enable();

            self.Move = inputSystem.GamePlay.Move;
            inputSystem.GamePlay.Cast_1.performed += _ =>
            {
                Room room = self.GetParent<Room>();
                room.Input.Button = 113;
            };
        }

        [EntitySystem]
        private static void Destroy(this LSFOperaComponent self)
        {
            self.InputSystem.Disable();
            self.InputSystem = null;
        }

        [EntitySystem]
        private static void Update(this LSFOperaComponent self)
        {
            Room room = self.GetParent<Room>();
            
            room.Input.V = self.Move.ReadValue<Vector2>().ToTSVector2().normalized;
        }
    }
}