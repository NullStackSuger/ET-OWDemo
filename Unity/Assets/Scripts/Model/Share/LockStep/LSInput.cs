using System;
using MemoryPack;
using TrueSync;

namespace ET
{
    [MemoryPackable]
    public partial struct LSInput
    {
        [MemoryPackOrder(0)]
        public TSVector2 V;
        
        [MemoryPackOrder(1)]
        public int Button;

        [MemoryPackOrder(2)]
        public bool Jump;

        [MemoryPackOrder(3)]
        public TSVector2 Look;

        public static void CopyPrediction(LSInput from, ref LSInput to)
        {
            to.V = from.V;
            to.Button = from.Button;
            to.Jump = from.Jump;
        }

        public static void CopyUnPrediction(LSInput from, ref LSInput to)
        {
            to.Look = from.Look;
        }

        public static void CopyUnPrediction(UnPredictionMessage from, ref LSInput to)
        {
            to.Look = from.Look;
        }
        
        public bool Equals(LSInput other)
        {
            return this.V == other.V && this.Button == other.Button && this.Jump == other.Jump && this.Look == other.Look;
        }

        public override bool Equals(object obj)
        {
            return obj is LSInput other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.V, this.Button, this.Jump, this.Look);
        }

        public static bool operator==(LSInput a, LSInput b)
        {
            if (a.V != b.V)
            {
                return false;
            }

            if (a.Button != b.Button)
            {
                return false;
            }

            if (a.Jump != b.Jump)
            {
                return false;
            }

            if (a.Look != b.Look)
            {
                return false;
            }

            return true;
        }

        public static bool operator !=(LSInput a, LSInput b)
        {
            return !(a == b);
        }

        public void Clear()
        {
            // V不清理, 因为移动需要连续, 其他的不用
            this.Button = 0;
            this.Jump = false;
        }
    }
}