using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET.Server
{

    public class SkeletonMoveHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            RigidBody body = unit.GetComponent<B3CollisionComponent>().Collision as RigidBody;
            // TODO 测试破坏球
            body.LinearVelocity = Vector3.Clamp(body.LinearVelocity, Vector3.One * -30, Vector3.One * 30);
            
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

            #region 破坏球的移动
            // TODO 测试破坏球
            body.ApplyCentralForce(offset.ToBullet() * 50);
            //body.LinearVelocity = Vector3.Clamp(body.LinearVelocity, Vector3.One * -30, Vector3.One * 30);
            // 应该还有手段保证绕圈, 不然很容易变成椭圆
            #endregion
            /*offset.y = body.LinearVelocity.Y;
            body.LinearVelocity = offset.ToBullet();*/
        }
    }
}