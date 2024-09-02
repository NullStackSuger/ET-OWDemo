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
            CollisionConfig config = CollisionConfigCategory.Instance.Get(configId);
            ACollisionCallback callback = CollisionCallbackDispatcherComponent.Instance[config.Callback];
            CollisionInfo info = CollisionInfoDispatcherComponent.Instance.Infos[configId];
            CollisionObject co = CollisionInfoDispatcherComponent.Instance.Clone(configId);
            
            self.Collision = co;
            co.UserObject = self;
            self.Offset = info.Position.ToTSVector();
            self.FollowUnitType = config.FollowUnitType;
         
            LSWorld world = self.IScene as LSWorld;
            B3WorldComponent worldComponent = world.GetComponent<B3WorldComponent>();
            worldComponent.NotifyToAdd(co, callback);

            LSUnit unit = self.GetParent<LSUnit>();
            self.Collision.WorldTransform += Matrix.Translation((float)unit.Position.x, (float)unit.Position.y, (float)unit.Position.z);
            //self.Body.Orientation += new Quaternion((float)unit.Rotation.x, (float)unit.Rotation.y, (float)unit.Rotation.z, (float)unit.Rotation.w);
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
            // TODO 这里还是有问题 移动 约束 mask
            
            if (self.FollowUnitType == 0) return;

            self.Collision.GetWorldTransform(out Matrix transform);
            LSUnit unit = self.GetParent<LSUnit>();

            if (self.FollowUnitType == -1)
            {
                unit.Position = new TSVector(transform.Origin.X, transform.Origin.Y, transform.Origin.Z) - self.Offset;
                unit.Rotation = new TSQuaternion(transform.Orientation.X, transform.Orientation.Y, transform.Orientation.Z, transform.Orientation.W);
            }
            else if (self.FollowUnitType == 1)
            {
                self.Collision.WorldTransform = Matrix.Translation(unit.Position.ToBullet());
            }
        }
    }
}