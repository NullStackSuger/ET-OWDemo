using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Room))]
    public class LSFCameraComponent : Entity, IAwake, IAwake<long>, ILateUpdate
    {
        public Camera Camera;
        public Transform LookAt;
        public EntityRef<LSUnit> Owner;

        public float InitFov;
        public readonly float ZoomLevel = 2.0f; // 倍镜倍数
        public readonly float ZoomInSpeed = 100.0f;
        public readonly float ZoomOutSpeed = 100.0f;
    }
}