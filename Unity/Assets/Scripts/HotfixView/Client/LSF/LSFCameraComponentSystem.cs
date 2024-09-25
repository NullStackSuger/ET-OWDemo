using System.Linq;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFCameraComponent))]
    [FriendOf(typeof(LSFCameraComponent))]
    [FriendOf(typeof(LSFUnitView))]
    [FriendOf(typeof(GlobalComponent))]
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
            LSFUnitView unitView;
            if (room.GetComponent<LSFUnitViewComponent>().Children.Count > 0)
            {
                unitView = room.GetComponent<LSFUnitViewComponent>().Children.First().Value as LSFUnitView;
            }
            else
            {
                unitView = room.GetComponent<LSFUnitViewComponent>().GetChild<LSFUnitView>(unitId);
            }
            Transform head = unitView.GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("Head").transform;

            self.Camera = Camera.main;
            self.LookAt = head;
            self.Owner = unitView.Unit;

            self.Camera.transform.parent = head;
            self.Camera.transform.localPosition = new Vector3(0, 0.1f, -0.15f);
            self.Camera.transform.localRotation = Quaternion.Euler(15, 0, 0);

            self.InitFov = self.Camera.fieldOfView;
        }

        [EntitySystem]
        private static void LateUpdate(this LSFCameraComponent self)
        {
            LSUnit owner = self.Owner;

            self.LookAt.localRotation = Quaternion.Lerp(self.LookAt.localRotation, Quaternion.Euler((float)owner.HeadRotation, 0, 0), 0.61f);
            
            // 设置相机视野
            if (UIHelper.Has(self.Root(), UIType.Snipe))
            {
                // 扩大视角
                self.Camera.ZoomIn(self.InitFov, self.ZoomLevel, self.ZoomInSpeed);
            }
            else
            {
                // 缩小视角
                self.Camera.ZoomOut(self.InitFov, self.ZoomOutSpeed);
            }
        }

        /// <summary>
        /// 扩大视角
        /// </summary>
        private static void ZoomIn(this Camera self, float initFov, float level, float speed)
        {
            if (Mathf.Abs(self.fieldOfView - (initFov / level)) < 0f)
            {
                self.fieldOfView = initFov / level;
            }
            else if (self.fieldOfView - (Time.deltaTime * speed) >= (initFov / level))
            {
                self.fieldOfView -= (Time.deltaTime * speed);
            }
        }

        /// <summary>
        /// 缩小视角
        /// </summary>
        private static void ZoomOut(this Camera self, float initFov, float speed)
        {
            if (Mathf.Abs(self.fieldOfView - initFov) < 0f)
            {
                self.fieldOfView = initFov;
            }
            else if (self.fieldOfView + (Time.deltaTime * speed) <= initFov)
            {
                self.fieldOfView += (Time.deltaTime * speed);
            }
        }
    }
}