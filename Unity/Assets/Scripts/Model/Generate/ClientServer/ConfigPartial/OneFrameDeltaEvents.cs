using System;
using System.Collections.Generic;

namespace ET
{
    // TODO 在客户端对收到的信息进行过滤, eg.A视角下没C, 但收到C的消息, 会把消息记录下来, 当A的视角里出现C, 再把收到的C的消息快速的作用给A视角的C
    public partial class OneFrameDeltaEvents
    {
        public void Add(string key, MessageObject value)
        {
            if (!this.Events.ContainsKey(key))
            {
                this.Events.Add(key, new LinkedList<MessageObject>());
            }
            this.Events[key].AddLast(value);
        }
        
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
                if (!b.Events.TryGetValue(kv.Key, out LinkedList<MessageObject> bEvents))
                {
                    return false;
                }

                LinkedList<MessageObject> aEvents = kv.Value;

                if (aEvents.Count != bEvents.Count)
                {
                    return false;
                }

                foreach (var aEvent in aEvents)
                {
                    if (!bEvents.Contains(aEvent))
                    {
                        return false;
                    }
                }
            }

            /*foreach (var kv in a.Events)
            {
                if (!b.Events.TryGetValue(kv.Key, out MessageObject bEvent))
                {
                    return false;
                }
                
                if (!kv.Value.Equals(bEvent))
                {
                    return false;
                }
            }*/

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