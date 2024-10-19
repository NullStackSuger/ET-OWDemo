using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(LSFUnitView))]
    public class LSFCameraComponent : Entity, IAwake, ILateUpdate
    {
        public Transform LookAt;
    }
}