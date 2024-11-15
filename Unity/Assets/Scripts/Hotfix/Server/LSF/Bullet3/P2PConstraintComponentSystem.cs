using System;
using BulletSharp;
using BulletSharp.Math;

namespace ET.Server
{
    [EntitySystemOf(typeof(P2PConstraintComponent))]
    [LSEntitySystemOf(typeof(P2PConstraintComponent))]
    [FriendOf(typeof(B3WorldComponent))]
    public static partial class P2PConstraintComponentSystem
    {
        [LSEntitySystem]
        private static void Awake(this P2PConstraintComponent self, Vector3 point, long length)
        {
            B3WorldComponent worldComponent = (self.IScene as LSWorld).GetComponent<B3WorldComponent>();
            RigidBody body = self.GetParent<B3CollisionComponent>().Collision as RigidBody;
            
            // TODO 测试破坏球
            Point2PointConstraint constraint = AddConstraint(worldComponent.World, body, point/*, length*/);

            self.Point = point;
            self.Length = length;
            self.Constraint = constraint;
        }
        [LSEntitySystem]
        private static void Destroy(this P2PConstraintComponent self)
        {
            B3WorldComponent worldComponent = (self.IScene as LSWorld).GetComponent<B3WorldComponent>();
            RemoveConstraint(worldComponent.World, self.Constraint);
        }

        public static void ChangeLength(this P2PConstraintComponent self, float length)
        {
            if (Math.Abs(length - self.Length) < 0.1f) return;
            
            B3WorldComponent worldComponent = (self.IScene as LSWorld).GetComponent<B3WorldComponent>();
            RigidBody body = self.GetParent<B3CollisionComponent>().Collision as RigidBody;
            
            // 移除之前约束
            RemoveConstraint(worldComponent.World, self.Constraint);
            
            // 添加约束
            Point2PointConstraint constraint = AddConstraint(worldComponent.World, body, self.Point, length);
            
            self.Length = (long)length;
            self.Constraint = constraint;
        }

        private static Point2PointConstraint AddConstraint(DynamicsWorld world, RigidBody body, Vector3 point, float length)
        {
            // 设置A的位置
            Vector3 position = body.WorldTransform.Origin;
            Vector3 ao = Vector3.Normalize(point - position) * length; // 向量OA
            //body.WorldTransform = Matrix.Translation(point - ao);
            
            // 添加约束
            return AddConstraint(world, body, ao);
        }

        private static Point2PointConstraint AddConstraint(DynamicsWorld world, RigidBody body, Vector3 point)
        {
            Point2PointConstraint constraint = new Point2PointConstraint(body, point);
            world.AddConstraint(constraint);

            return constraint;
        }

        private static void RemoveConstraint(DynamicsWorld world, Point2PointConstraint constraint)
        {
            if (constraint == null) return;
            world.RemoveConstraint(constraint);
        }
    }
}