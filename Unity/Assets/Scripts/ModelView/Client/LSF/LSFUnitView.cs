using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(LSFUnitViewComponent))]
    public class LSFUnitView : Entity, IAwake<string, GameObject, LSUnit>, IAwake<GameObject, LSUnit>, IUpdate, IDestroy, ILSRollback
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
        
        public float TotalTime;
        public float Time;
    }
}