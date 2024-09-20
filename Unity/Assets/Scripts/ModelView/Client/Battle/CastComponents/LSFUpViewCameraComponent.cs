using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Room))]
    public class LSFUpViewCameraComponent : Entity, IAwake<LSUnit>, ILateUpdate
    {
        public Camera Camera;
        public Transform LookAt;
        public EntityRef<LSUnit> Owner;
    }
}