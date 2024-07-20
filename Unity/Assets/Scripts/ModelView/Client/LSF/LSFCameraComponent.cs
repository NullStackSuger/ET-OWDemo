using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Room))]
    public class LSFCameraComponent : Entity, IAwake, ILateUpdate
    {
        public Camera Camera;
        public Transform LookAt;
    }
}