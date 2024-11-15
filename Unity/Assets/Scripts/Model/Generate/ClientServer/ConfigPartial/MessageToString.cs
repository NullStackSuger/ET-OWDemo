namespace ET
{
    public partial class S2C_UnitChangePosition
    {
        public override string ToString()
        {
            return $"{this.PlayerId}_{nameof(S2C_UnitChangePosition)}";
        }
    }

    public partial class S2C_UnitChangeRotation
    {
        public override string ToString()
        {
            return $"{this.PlayerId}_{nameof(S2C_UnitChangeRotation)}";
        }
    }

    public partial class S2C_UnitChangeHeadRotation
    {
        public override string ToString()
        {
            return $"{this.PlayerId}_{nameof(S2C_UnitChangeHeadRotation)}";
        }
    }

    public partial class S2C_UnitUseCast
    {
        public override string ToString()
        {
            return $"{this.PlayerId}_{nameof(S2C_UnitUseCast)}";
        }
    }

    public partial class S2C_UnitRemoveCast
    {
        public override string ToString()
        {
            return $"{this.PlayerId}_{nameof(S2C_UnitRemoveCast)}";
        }
    }

    public partial class S2C_UnitUseBuff
    {
        public override string ToString()
        {
            return $"{this.PlayerId}_{nameof(S2C_UnitUseBuff)}";
        }
    }

    public partial class S2C_UnitRemoveBuff
    {
        public override string ToString()
        {
            return $"{this.PlayerId}_{nameof(S2C_UnitRemoveBuff)}";
        }
    }

    public partial class S2C_UnitChangeDataModifier
    {
        public override string ToString()
        {
            return $"{this.PlayerId}_{nameof(S2C_UnitChangeDataModifier)}";
        }
    }

    public partial class S2C_UnitOnGround
    {
        public override string ToString()
        {
            return $"{this.PlayerId}_{nameof(S2C_UnitOnGround)}";
        }
    }
}