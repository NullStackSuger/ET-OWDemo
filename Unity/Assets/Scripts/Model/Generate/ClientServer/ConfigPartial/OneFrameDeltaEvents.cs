using System;

namespace ET
{
    public partial class OneFrameDeltaEvents
    {
        public void CopyTo(OneFrameDeltaEvents to)
        {
            to.Events.Clear();
            foreach (var kv in this.Events)
            {
                to.Events.Add(kv.Key, kv.Value);
            }
        }

        public void Clear()
        {
            this.Events.Clear();
            this.Frame = default;
        }

        public static bool operator ==(OneFrameDeltaEvents a, OneFrameDeltaEvents b)
        {
            if (a is null || b is null)
            {
                if (a is null && b is null)
                {
                    return true;
                }
                return false;
            }
            
            if (a.Events.Count != b.Events.Count)
            {
                return false;
            }

            foreach (var kv in a.Events)
            {
                if (!b.Events.TryGetValue(kv.Key, out MessageObject bEvent))
                {
                    return false;
                }

                if (!kv.Value.Equals(bEvent))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(OneFrameDeltaEvents a, OneFrameDeltaEvents b)
        {
            return !(a == b);
        }
        
        protected bool Equals(OneFrameDeltaEvents other)
        {
            return Equals(this.Events, other.Events);
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

            return Equals((OneFrameDeltaEvents) obj);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Events);
        }
    }
}