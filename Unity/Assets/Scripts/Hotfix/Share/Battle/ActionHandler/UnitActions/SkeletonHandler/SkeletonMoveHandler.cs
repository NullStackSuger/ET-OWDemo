using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET
{

    public class SkeletonMoveHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            RigidBody body = unit.GetComponent<B3CollisionComponent>().Collision as RigidBody;
            
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;
            if (TSMath.Abs(input.V.x) < 0.1f && TSMath.Abs(input.V.y) < 0.1f)
            {
                body.LinearVelocity = new Vector3(0, body.LinearVelocity.Y, 0);
                return false;
            }

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            RigidBody body = unit.GetComponent<B3CollisionComponent>().Collision as RigidBody;
            
            TSMatrix matrix = TSMath.RotationMatrix(unit.HeadRotation, unit.Rotation);
            float speed = dataModifierComponent.Get(DataModifierType.Speed);
            TSVector offset = matrix * new TSVector(input.V.x, 0, input.V.y) * speed;
            offset.y = body.LinearVelocity.Y;
            body.LinearVelocity = offset.ToBullet(); 
        }
    }
}