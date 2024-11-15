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
            LSFUnitView unitView = self.GetParent<LSFUnitView>();
            
            Transform head = unitView.GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("Head").transform;
            self.LookAt = head;
            
            Camera.main.transform.parent = head;
            Camera.main.transform.localPosition = new Vector3(0, 0.1f, -0.15f);
            Camera.main.transform.localRotation = Quaternion.Euler(15, 0, 0);
        }

        [EntitySystem]
        private static void LateUpdate(this LSFCameraComponent self)
        {
            LSUnit owner = self.GetParent<LSFUnitView>().Unit;

            self.LookAt.localRotation = Quaternion.Euler(Mathf.Lerp(self.LookAt.localRotation.x, (float)owner.HeadRotation, 0.35f), 0, 0);
        }
    }
}