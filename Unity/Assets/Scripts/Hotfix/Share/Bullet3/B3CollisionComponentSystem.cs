using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(B3CollisionComponent))]
    [LSEntitySystemOf(typeof(B3CollisionComponent))]
    [FriendOf(typeof(B3CollisionComponent))]
    [FriendOf(typeof(B3WorldComponent))]
    public static partial class B3CollisionComponentSystem
    {
        [EntitySystem]
        private static void Awake(this B3CollisionComponent self, int configId)
        {
            LSWorld world = self.IScene as LSWorld;
            B3WorldComponent worldComponent = world.GetComponent<B3WorldComponent>();
            LSUnit unit = self.GetParent<LSUnit>();
            
            CollisionConfig config = CollisionConfigCategory.Instance.Get(configId);
            ACollisionCallback callback = CollisionCallbackDispatcherComponent.Instance[config.Callback];
            CollisionInfo info = CollisionInfoDispatcherComponent.Instance.Infos[configId];
            CollisionObject co = CollisionInfoDispatcherComponent.Instance.Clone(configId);
            
            self.Collision = co;
            co.UserObject = self;
            self.Offset = info.Position;
            
            worldComponent.NotifyToAdd(co, callback);

            self.Collision.WorldTransform += Matrix.Translation(unit.Position.ToVector());
            /*Matrix transform = self.Collision.WorldTransform;
            transform = Matrix.Translation(unit.Position.ToVector());
            Quaternion quaternionY = new Quaternion(Vector3.UnitY, -MathUtil.DegToRadians((float)unit.Rotation));
            Quaternion quaternionX = new Quaternion(Vector3.UnitX, -MathUtil.DegToRadians((float)unit.HeadRotation));
            transform.Orientation = quaternionY * quaternionX;
            self.Collision.WorldTransform = transform;*/
        }
        
        [EntitySystem]
        private static void Destroy(this B3CollisionComponent self)
        {
            LSWorld world = self.IScene as LSWorld;
            B3WorldComponent worldComponent = world.GetComponent<B3WorldComponent>();
            worldComponent.NotifyToRemove(self.Collision);
            
            self.Collision = null;
        }

        [LSEntitySystem]
        private static void LSUpdate(this B3CollisionComponent self)
        {
            Vector3 position = self.Collision.WorldTransform.Origin;
            LSUnit unit = self.GetParent<LSUnit>();
            
            unit.Position = (position - self.Offset).ToTSVector();
        }

        #region MoveTo
        public static void MoveTo(this B3CollisionComponent self, Vector3 position)
        {
            self.MoveTo(Matrix.Translation(position));
        }
        public static void MoveTo(this B3CollisionComponent self, Vector3 position, Quaternion rotation)
        {
            Matrix transform = Matrix.Translation(position);
            transform.Orientation = rotation;
            self.MoveTo(transform);
        }
        public static void MoveTo(this B3CollisionComponent self, Vector3 position, float rotation, float headRotation)
        {
            Quaternion quaternionY = new Quaternion(Vector3.UnitY, -MathUtil.DegToRadians(rotation));
            Quaternion quaternionX = new Quaternion(Vector3.UnitX, -MathUtil.DegToRadians(headRotation));
            self.MoveTo(position, quaternionY * quaternionX);
            
            //Log.Warning($"{self.Collision.WorldTransform.Orientation}");
        }
        public static void MoveTo(this B3CollisionComponent self, Matrix matrix)
        {
            self.Collision.WorldTransform = matrix;
        }
        #endregion
    }
}