using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET.Server
{
    [FriendOf(typeof(B3WorldComponent))]
    [FriendOf(typeof(ActionComponent))]
    public class GrappleInitHandler : AActionHandler
    {
        private const long GrappleMaxLength = 5;
        
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

            TSMatrix matrix = TSMath.RotationMatrix(owner.HeadRotation, owner.Rotation);
            Vector3 offset = (matrix * TSVector.forward * GrappleMaxLength).ToBullet();
            Vector3 startPos = owner.Position.ToBullet();
            bool hasHit = worldComponent.RayTestFirst(startPos, startPos + offset, out ClosestRayResultCallback callback);
            if (!hasHit)
            {
                //callback.HitPointWorld = startPos + offset;
                return;
            }
            Vector3 endPos = callback.HitPointWorld;
            actionComponent.Args.Add("EndPos", endPos);
            
            B3CollisionComponent collisionComponent = owner.GetComponent<B3CollisionComponent>();
            // TODO 测试破坏球
            P2PConstraintComponent constraint = collisionComponent.AddComponent<P2PConstraintComponent, Vector3, long>(/*endPos*/new Vector3(0, 10, 10), (long)Vector3.Distance(endPos, startPos));
            actionComponent.Args.Add("Constraint", constraint);
        }
    }
    
    [FriendOf(typeof(ActionComponent))]
    public class GrappleHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            Vector3 startPos = owner.Position.ToBullet();
            Vector3 endPos = (Vector3)actionComponent.Args["EndPos"];
            return Vector3.Distance(startPos, endPos) > 0.2;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            P2PConstraintComponent constraint = actionComponent.Args["Constraint"] as P2PConstraintComponent;
            //constraint.ChangeLength(constraint.Length * 0.5f);
            //Log.Warning($"ChangeLength");
        }
    }

    public class GrappleEndHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            owner.GetComponent<B3CollisionComponent>().RemoveComponent<P2PConstraintComponent>();
            
            CastComponent castComponent = owner.GetComponent<CastComponent>();
            castComponent.Remove(unit.Id);
        }
    }
}