using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFUnitView))]
    [LSEntitySystemOf(typeof(LSFUnitView))]
    [FriendOf(typeof(LSFUnitView))]
    [FriendOf(typeof(GlobalComponent))]
    public static partial class LSFUnitViewSystem
    {
        [EntitySystem]
        private static void Awake(this LSFUnitView self, string type, GameObject obj, LSUnit unit)
        {
            obj.transform.position = unit.Position.ToVector();
            obj.transform.rotation = Quaternion.Euler(0, (float)unit.Rotation, 0);

            self.GameObject = obj;
            self.Transform = obj.transform;
            self.Unit = unit;

            if (obj.TryGetComponent<Animator>(out Animator animator))
            {
                self.AddComponent<LSFAnimatorComponent, string>(type);
            }
        }

        [EntitySystem]
        private static void Awake(this LSFUnitView self, GameObject obj, LSUnit unit)
        {
            obj.transform.position = unit.Position.ToVector();
            obj.transform.rotation = Quaternion.Euler(0, (float)unit.Rotation, 0);

            self.GameObject = obj;
            self.Transform = obj.transform;
            self.Unit = unit;
        }

        [EntitySystem]
        private static void Destroy(this LSFUnitView self)
        {
            if (Camera.main.transform.IsChildOf(self.Transform))
            {
                Camera.main.transform.parent = self.Root().GetComponent<GlobalComponent>().Global;
            }
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

            if (position != self.Position)
            {
                float distance = (position - self.Position).magnitude;
                self.TotalTime = distance / LSFConfig.Speed;
                self.Time = 0;

                self.Position = position;
            }

            self.Time += Time.deltaTime;
            self.Transform.position = Vector3.Lerp(self.Transform.position, self.Position, self.Time / self.TotalTime);
            self.Transform.rotation = Quaternion.Lerp(self.Transform.rotation, Quaternion.Euler(0, (float)unit.Rotation, 0), self.Time / 0.51f);
        }

        [LSEntitySystem]
        private static void LSRollback(this LSFUnitView self)
        {
            LSUnit unit = self.Unit;
            self.Transform.position = unit.Position.ToVector();
            self.Transform.rotation = Quaternion.Euler(0, (float)unit.Rotation, 0);
            self.Time = 0;
            self.TotalTime = 0;
        }
    }
}