using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(LSFUnitViewComponent))]
    public class LSFUnitView : Entity, IAwake<AnimatorType, GameObject, LSUnit>, IAwake<GameObject, LSUnit>, IUpdate, IDestroy, ILSRollback
    {
        public EntityRef<LSUnit> Owner;
        
        public EntityRef<LSUnit> Unit;
        public GameObject GameObject;
        public Transform Transform;

        public Vector3 Position;
        public Quaternion Rotation;
        public float TotalTime;
        public float Time;
    }
}