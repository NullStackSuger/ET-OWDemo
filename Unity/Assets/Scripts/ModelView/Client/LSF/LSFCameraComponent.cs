using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Room))]
    public class LSFCameraComponent : Entity, IAwake, IAwake<long>, ILateUpdate
    {
        public Camera Camera;
        public Transform LookAt;
        public EntityRef<LSUnit> Owner;
    }
}