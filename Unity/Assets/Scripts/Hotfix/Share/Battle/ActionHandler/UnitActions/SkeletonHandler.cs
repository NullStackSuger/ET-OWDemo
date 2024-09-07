using System;
using System.Linq;
using BulletSharp;
using BulletSharp.Math;
using TrueSync;

namespace ET
{
    [FriendOfAttribute(typeof(ET.BuffComponent))]
    [FriendOfAttribute(typeof(ET.DataModifierComponent))]
    public class SkeletonHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            /*LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;
            return input.V != TSVector2.zero || input.Button != 0;*/
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;

            MoveHandler(input, unit);
            CastHandler(input, unit);
        }

        private static void MoveHandler(LSInput input, LSUnit unit)
        {
            RigidBody body = unit.GetComponent<B3CollisionComponent>().Collision as RigidBody;
            
            TSVector2 v2 = input.V * 6 * 50 / 1000;
            if (TSMath.Abs(v2.x) < 0.1f && TSMath.Abs(v2.y) < 0.1f)
            {
                body.LinearVelocity = Vector3.Zero;
                return;
            }
            
            DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();

            /*TSVector oldPos = unit.Position;
            unit.Position += new TSVector(v2.x, 0, v2.y) * dataModifierComponent.Get(DataModifierType.Speed);
            unit.Forward = unit.Position - oldPos;
            unit.Forward = new TSVector(v2.x, 0, v2.y);*/
            
            body.LinearVelocity = new Vector3((float)v2.x, (float)unit.Position.y, (float)v2.y) * dataModifierComponent.Get(DataModifierType.Speed);
            //body.LinearVelocity = Vector3.Zero;
            //body.ApplyCentralForce(new Vector3((float)v2.x, (float)unit.Position.y, (float)v2.y) * dataModifierComponent.Get(DataModifierType.Speed));
        }

        private static void CastHandler(LSInput input, LSUnit unit)
        {
            if (input.Button == 0) return;

            LSWorld world = unit.IScene as LSWorld;
            Room room = world.GetParent<Room>();
            LSFInputComponent inputComponent = unit.GetComponent<LSFInputComponent>();

            long now = room.FixedTimeCounter.FrameTime(room.AuthorityFrame);
            long last = room.FixedTimeCounter.FrameTime(inputComponent.PressCastFrame);
            if (now - last < 1000) return;

            inputComponent.PressCastFrame = room.AuthorityFrame;

            CastComponent castComponent = unit.GetComponent<CastComponent>();
            castComponent.Creat(1001);
            
            /*BuffComponent buffComponent = unit.GetComponent<BuffComponent>();
            buffComponent.Creat(1001);*/
            
            /*DataModifierComponent dataModifierComponent = unit.GetComponent<DataModifierComponent>();
            long atk = dataModifierComponent.Get(DataModifierType.Atk);
            DataModifierHelper.DefaultBattle(atk, dataModifierComponent, DataModifierType.Hp);

            ConstantModifier hpAdd = new Default_Hp_ConstantModifier();
            FinalConstantModifier hpFinalAdd = new Default_Hp_FinalConstantModifier();
            dataModifierComponent.Clear(DataModifierType.Hp, ref hpAdd, ref hpFinalAdd);
            
            Log.Warning($"完成Atk, 当前生命为{dataModifierComponent.Get(DataModifierType.Hp)}, 当前数目为{dataModifierComponent.NumericDic[DataModifierType.Hp].Count}");*/
        }
    }
}