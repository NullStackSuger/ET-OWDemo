using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(LSFUnitViewComponent))]
    public class LSFUnitView : Entity, IAwake<GameObject, LSUnit>, IUpdate, IDestroy, ILSRollback
    {
        private EntityRef<LSUnit> unit;
        public EntityRef<LSUnit> Unit
        {
            get
            {
                LSUnit unit = this.unit;
                if (unit != null) return unit;
                
                unit = (this.IScene as Room).PredictionWorld.GetComponent<LSUnitComponent>().GetChild<LSUnit>(this.Id);
                if (unit != null)
                {
                    this.Unit = unit;
                    Log.Warning($"{this.Root().Name}更新InstanceId, {unit.Id}, {unit.InstanceId}");
                    return unit;
                }

                return null;
            }
            set
            {
                LSUnit newUnit = value;
                LSUnit oldUnit = this.unit;
                if (newUnit == null) Log.Error("值不能为空");
                if (newUnit == oldUnit) return;
                this.unit = value;
            }
        }
        public GameObject GameObject;
        public Transform Transform;
        
        public Vector3 Position;
    }
}