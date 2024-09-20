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
            self.Look = inputSystem.GamePlay.Look;
            
            inputSystem.GamePlay.Cast_E.performed += _ =>
            {
                // 如果把room放在外面可能有闭包的问题
                Room room = self.GetParent<Room>();
                room.Input.Button = 101;
            };
            
            inputSystem.GamePlay.Cast_Q.performed += _ =>
            {
                // 如果把room放在外面可能有闭包的问题
                Room room = self.GetParent<Room>();
                room.Input.Button = 113;
            };
            
            inputSystem.GamePlay.Cast_C.performed += _ =>
            {
                // 如果把room放在外面可能有闭包的问题
                Room room = self.GetParent<Room>();
                room.Input.Button = 99;
            };

            inputSystem.GamePlay.Jump.performed += _ =>
            {
                Room room = self.GetParent<Room>();
                room.Input.Jump = true;
            };

            inputSystem.GamePlay.ChangeBulletCount.performed += _ =>
            {
                Room room = self.GetParent<Room>();
                room.Input.Button = 114;
            };
            
            inputSystem.GamePlay.Interactive.performed += _ =>
            {
                Room room = self.GetParent<Room>();
                room.Input.Button = 118;
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

            TSVector2 look = self.Look.ReadValue<Vector2>().ToTSVector2().normalized;
            look.y *= -1;
            
            // 发送消息
            Scene root = room.Root();
            UnPredictionMessage unPredictionMessage = UnPredictionMessage.Create();
            unPredictionMessage.Frame = room.PredictionFrame + 2;
            unPredictionMessage.PlayerId = room.PlayerId;
            unPredictionMessage.Look = look;
            root.GetComponent<ClientSenderComponent>().Send(unPredictionMessage);
            Log.Warning($"发送不预测消息:{unPredictionMessage.Frame}, {unPredictionMessage.Look}");   
        }
    }
}