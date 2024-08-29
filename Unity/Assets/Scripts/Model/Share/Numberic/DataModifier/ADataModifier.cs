using System;

namespace ET
{
    public abstract class ADataModifier : Object, IDisposable
    {
        public abstract int ModifierType { get; }

        public abstract int Key { get; }

        private long value;
        public long Value
        {
            set
            {
                Set(value);       
            }
        }

        public bool NeedClear { get; set; }

        public virtual void Set(long val)
        {
            this.value = val;
        }

        public virtual long Get(DataModifierComponent self)
        {
            return this.value;
        }
        
        public virtual void Dispose()
        {
            
        }
    }
}