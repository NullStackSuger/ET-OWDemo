using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET.Server
{
    [FriendOfAttribute(typeof(B3WorldComponent))]
    [FriendOfAttribute(typeof(ET.ActionComponent))]
    public class GrapplerInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            // 添加射线检测
            LSWorld world = actionComponent.IScene as LSWorld;
            B3WorldComponent worldComponent = world.GetComponent<B3WorldComponent>();

            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            B3CollisionComponent collisionComponent = owner.GetComponent<B3CollisionComponent>();

            TSMatrix matrix = TSMath.RotationMatrix(owner.HeadRotation, owner.Rotation);
            Vector3 length = (matrix * TSVector.forward * 10).ToBullet();
            Vector3 startPos = owner.Position.ToBullet();
            world.GetComponent<B3WorldComponent>().RayTestFirst(startPos, startPos + length, out ClosestRayResultCallback callback);
            Vector3 endPos = callback.HitPointWorld;
            actionComponent.Args.Add("EndPos", endPos);
            
            Point2PointConstraint p2pConstraint = new Point2PointConstraint(collisionComponent.Collision as RigidBody, endPos);
            worldComponent.World.AddConstraint(p2pConstraint);
            actionComponent.Args.Add("Constraint", p2pConstraint);
        }
    }
    
    [FriendOfAttribute(typeof(ET.ActionComponent))]
    public class GrapplerHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            Vector3 endPos = (Vector3)actionComponent.Args["EndPos"];

            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            Vector3 startPos = owner.Position.ToBullet();

            return Vector3.Distance(startPos, endPos) >= 0.1;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            /*// 每次缩短一半, 直到距离小于0.1
            Point2PointConstraint p2pConstraint = new Point2PointConstraint(collisionComponent.Collision as RigidBody, endPos);
            //worldComponent.World.AddConstraint();
            //worldComponent.World.RemoveConstraint();*/
        }
    }

    public class GrapplerEndHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            CastComponent castComponent = owner.GetComponent<CastComponent>();
            castComponent.Remove(unit.Id);
        }
    }
}