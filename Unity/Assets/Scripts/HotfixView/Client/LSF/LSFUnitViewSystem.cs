using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFUnitView))]
    [LSEntitySystemOf(typeof(LSFUnitView))]
    [FriendOf(typeof(LSFUnitView))]
    public static partial class LSFUnitViewSystem
    {
        [EntitySystem]
        private static void Awake(this LSFUnitView self, AnimatorType type, GameObject obj, LSUnit unit)
        {
            obj.transform.position = unit.Position.ToVector();
            obj.transform.rotation = unit.Rotation.ToQuaternion();
            
            self.GameObject = obj;
            self.Transform = obj.transform;
            self.Unit = unit;
            self.AddComponent<LSFAnimatorComponent, AnimatorType>(type);
        }
        
        [EntitySystem]
        private static void Awake(this LSFUnitView self, GameObject obj, LSUnit unit)
        {
            obj.transform.position = unit.Position.ToVector();
            obj.transform.rotation = unit.Rotation.ToQuaternion();
            
            self.GameObject = obj;
            self.Transform = obj.transform;
            self.Unit = unit;
        }
        
        [EntitySystem]
        private static void Destroy(this LSFUnitView self)
        {
            UnityEngine.Object.Destroy(self.GameObject);
        }

        [EntitySystem]
        private static void Update(this LSFUnitView self)
        {
            LSUnit unit = self.Unit;
            if (unit == null)
            {
                LSFUnitViewComponent unitViewComponent = self.GetParent<LSFUnitViewComponent>();
                unitViewComponent.Remove(self.Id);
                return;
            }
            
            Vector3 position = unit.Position.ToVector();
            Quaternion rotation = unit.Rotation.ToQuaternion();

            if (position != self.Transform.position /*因为转向必须移动, 这里不用判断旋转*/)
            {
                float distance = (position - self.Position).magnitude;
                self.TotalTime = distance / LSFConfig.Speed;
                self.Time = 0;
                
                self.Position = position;
                self.Rotation = rotation;
            }

            self.Time += Time.deltaTime;
            // 会看见权威Unit来回闪是因为KCP丢包导致下一个先到了 改变位置, 而因为重发机制 又把当前包重发 把Unit拉回来了 下第2个包没丢导致位置跨度比较大
            self.Transform.position = Vector3.Lerp(self.Transform.position, self.Position, self.Time / self.TotalTime);
            self.Transform.rotation = Quaternion.Lerp(self.Transform.rotation, self.Rotation, self.Time / 1f);
        }

        [LSEntitySystem]
        private static void LSRollback(this LSFUnitView self)
        {
            LSUnit unit = self.Unit;
            self.Transform.position = unit.Position.ToVector();
            self.Transform.rotation = unit.Rotation.ToQuaternion();
            self.Time = 0;
            self.TotalTime = 0;
        }
    }
}