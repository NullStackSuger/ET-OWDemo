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
            self.Awake(room.PlayerId);
        }
        
        [EntitySystem]
        private static void Awake(this LSFCameraComponent self, long unitId)
        {
            Room room = self.GetParent<Room>();
            LSFUnitView unitView = room.GetComponent<LSFUnitViewComponent>().GetChild<LSFUnitView>(unitId);
            Transform head = unitView.GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("Head").transform;
            
            // TODO 这里有时会出现Camera.main == null的情况
            self.Camera = Camera.main;
            self.LookAt = head;
            self.Owner = unitView.Unit;
            
            self.Camera.transform.parent = head;
            self.Camera.transform.localPosition = new Vector3(0, 0.1f, -0.15f);
            self.Camera.transform.localRotation = Quaternion.Euler(15, 0, 0);
        }

        [EntitySystem]
        private static void LateUpdate(this LSFCameraComponent self)
        {
            LSUnit owner = self.Owner;

            self.LookAt.localRotation = Quaternion.Lerp(self.LookAt.localRotation, Quaternion.Euler((float)owner.HeadRotation, 0, 0), 0.61f);
        }
    }
}