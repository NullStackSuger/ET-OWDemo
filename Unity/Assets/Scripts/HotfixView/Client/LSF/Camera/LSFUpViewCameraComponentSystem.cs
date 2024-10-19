using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFUpViewCameraComponent))]
    [FriendOf(typeof(LSFUnitView))]
    public static partial class LSFUpViewCameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSFUpViewCameraComponent self)
        {
            LSFUnitView unitView = self.GetParent<LSFUnitView>();
            
            Camera.main.transform.parent = unitView.Transform;
            Camera.main.transform.localPosition = new Vector3(0, 10f, 0);
            Camera.main.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
    }
}