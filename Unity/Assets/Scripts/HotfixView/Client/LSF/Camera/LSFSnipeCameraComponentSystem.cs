using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFSnipeCameraComponent))]
    [FriendOfAttribute(typeof(ET.Client.LSFSnipeCameraComponent))]
    public static partial class LSFSnipeCameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.LSFSnipeCameraComponent self)
        {
            self.Camera = Camera.main;
            self.InitFov = self.Camera.fieldOfView;
        }
        [EntitySystem]
        private static void LateUpdate(this ET.Client.LSFSnipeCameraComponent self)
        {
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