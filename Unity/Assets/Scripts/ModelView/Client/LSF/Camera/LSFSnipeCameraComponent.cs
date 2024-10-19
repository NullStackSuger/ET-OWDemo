using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(LSFCameraComponent))]
    public class LSFSnipeCameraComponent : Entity, IAwake, ILateUpdate
    {
        public Camera Camera;
        
        public float InitFov;
        public readonly float ZoomLevel = 2.0f; // 倍镜倍数
        public readonly float ZoomInSpeed = 100.0f;
        public readonly float ZoomOutSpeed = 100.0f;
    }
}