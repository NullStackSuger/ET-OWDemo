using System.Linq;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFCameraComponent))]
    [FriendOf(typeof(LSFCameraComponent))]
    [FriendOf(typeof(LSFUnitView))]
    public static partial class LSFCameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSFCameraComponent self)
        {
            Room room = self.GetParent<Room>();
            LSFUnitView unitView = room.GetComponent<LSFUnitViewComponent>().Children.First().Value as LSFUnitView;

            self.Camera = Camera.main;
            self.LookAt = unitView.Transform;
            self.Camera.transform.rotation = Quaternion.Euler(new Vector3(20, 0, 0));
        }

        [EntitySystem]
        private static void LateUpdate(this LSFCameraComponent self)
        {
            if (self.LookAt == null) return;

            self.Camera.transform.position = self.LookAt.position + new Vector3(0, 3, -5);
        }
    }
}