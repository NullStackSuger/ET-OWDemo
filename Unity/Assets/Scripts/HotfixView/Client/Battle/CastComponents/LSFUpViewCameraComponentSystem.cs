using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFUpViewCameraComponent))]
    [FriendOf(typeof(LSFUnitView))]
    [FriendOf(typeof(LSFUpViewCameraComponent))]
    public static partial class LSFUpViewCameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSFUpViewCameraComponent self, LSUnit unit)
        {
            Room room = self.GetParent<Room>();
            LSFUnitView unitView = room.GetComponent<LSFUnitViewComponent>().GetChild<LSFUnitView>(unit.Id);

            self.Camera = Camera.main;
            self.LookAt = unitView.Transform;
            self.Owner = unit;
            
            self.Camera.transform.parent = unitView.Transform;
            self.Camera.transform.localPosition = new Vector3(0, 10f, 0);
            self.Camera.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
        [EntitySystem]
        private static void LateUpdate(this LSFUpViewCameraComponent self)
        {

        }
    }
}