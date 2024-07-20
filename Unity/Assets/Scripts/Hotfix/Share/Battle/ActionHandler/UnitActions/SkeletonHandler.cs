using TrueSync;

namespace ET
{

    public class SkeletonHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSInput input = unit.GetComponent<LSFInputComponent>().Input;
            return input.V != TSVector2.zero || input.Button != 0;
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
            TSVector2 v2 = input.V * 6 * 50 / 1000;
            if (v2.LengthSquared() < 0.0001f)
            {
                return;
            }
            TSVector oldPos = unit.Position;
            unit.Position += new TSVector(v2.x, 0, v2.y);
            unit.Forward = unit.Position - oldPos;
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
            Cast cast = castComponent.Creat(1003, new TestContact());
        }
    }
}