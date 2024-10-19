namespace ET.Server
{
    [FriendOf(typeof(ActionComponent))]
    public class ShieldInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            /*LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            Cast cast = owner.GetComponent<CastComponent>().GetChild<Cast>(unit.Id);
            CastConfig castConfig = CastConfigCategory.Instance.Get(cast.ConfigId);
            actionComponent.Args.Add("Offset", new TSVector(castConfig.X, castConfig.Y, castConfig.Z));*/
        }
    }
    [FriendOf(typeof(ActionComponent))]
    public class ShieldHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            /*LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            CastComponent castComponent = owner.GetComponent<CastComponent>();*/

            /*LSInput input = owner.GetComponent<LSFInputComponent>().Input;
            if (input.Button != 113)
            {
                castComponent.Remove(unit.Id);
                return false;
            }*/
            
            /*DataModifierComponent dataModifierComponent = owner.GetComponent<DataModifierComponent>();
            if (dataModifierComponent.Get(DataModifierType.Shield) <= 0)
            {
                castComponent.Remove(unit.Id);
                return false;
            }*/

            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            /*LSUnit unit = actionComponent.GetParent<LSUnit>();
            LSUnit owner = unit.Owner;
            B3CollisionComponent collisionComponent = unit.GetComponent<B3CollisionComponent>();*/

            /*unit.Rotation = owner.Rotation;
            unit.HeadRotation = owner.HeadRotation;
            
            TSMatrix matrix = TSMath.RotationMatrix(0, owner.Rotation);
            collisionComponent.MoveTo(
                (owner.Position + matrix * (TSVector)actionComponent.Args["Offset"]).ToBullet(), 
                (float)unit.Rotation, (float)unit.HeadRotation);*/
        }
    }

    public class ShieldEndHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            
        }
    }
}