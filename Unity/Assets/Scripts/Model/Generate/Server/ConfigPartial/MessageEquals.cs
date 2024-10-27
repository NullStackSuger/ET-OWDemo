using System;
using TrueSync;

namespace ET
{
    public partial class S2C_UnitChangePosition
    {
        protected bool Equals(S2C_UnitChangePosition other)
        {
            return this.UnitId == other.UnitId && TSVector.Distance(this.Position, other.Position) < 0.05f;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((S2C_UnitChangePosition) obj);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(this.UnitId, this.Position);
        }
    }
    
    public partial class S2C_UnitChangeRotation
    {
        protected bool Equals(S2C_UnitChangeRotation other)
        {
            return this.UnitId == other.UnitId && /*this.Rotation == other.Rotation*/ TSMath.Abs(this.Rotation - other.Rotation) < 3;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((S2C_UnitChangeRotation) obj);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(this.UnitId, this.Rotation);
        }
    }

    public partial class S2C_UnitChangeHeadRotation
    {
        protected bool Equals(S2C_UnitChangeHeadRotation other)
        {
            return this.UnitId == other.UnitId && /*this.HeadRotation == other.HeadRotation*/ TSMath.Abs(this.HeadRotation - other.HeadRotation) < 3;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((S2C_UnitChangeHeadRotation) obj);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(this.UnitId, this.HeadRotation);
        }
    }
}