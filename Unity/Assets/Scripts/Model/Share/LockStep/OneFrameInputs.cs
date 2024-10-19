using System;
using System.Collections.Generic;
using TrueSync;

namespace ET
{
    public partial class OneFrameInputs
    {
        protected bool Equals(OneFrameInputs other)
        {
            return Equals(this.Inputs, other.Inputs);
        }

        public void CopyTo(OneFrameInputs to)
        {
            to.Inputs.Clear();
            foreach (var kv in this.Inputs)
            {
                to.Inputs.Add(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// 复制除id相同之外的成员
        /// </summary>
        public void CopyTo(OneFrameInputs to, long id)
        {
            LSInput input = to.Inputs[id];
            to.Inputs.Clear();
            
            foreach (var kv in this.Inputs)
            {
                if (kv.Key == id)
                {
                    to.Inputs.Add(id, input);
                    continue;
                }
                to.Inputs.Add(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// 拷贝可预测的值
        /// </summary>
        public void CopyToPrediction(OneFrameInputs to)
        {
            foreach (var kv in this.Inputs)
            {
                if (to.Inputs.TryGetValue(kv.Key, out LSInput input))
                {
                    LSInput.CopyPrediction(kv.Value, ref input);
                    
                    to.Inputs[kv.Key] = input;
                }
                else
                {
                    to.Inputs[kv.Key] = kv.Value;
                }
            }
        }

        /// <summary>
        /// 把value的Prediction值给playerInput,
        /// 把playerInput的UnPrediction值给value
        /// </summary>
        public void CopyEach(long playerId, ref LSInput value)
        {
            this.Inputs.TryAdd(playerId, value);
            LSInput input = this.Inputs[playerId];
            
            LSInput.CopyPrediction(value, ref input);
            LSInput.CopyUnPrediction(input, ref value);

            this.Inputs[playerId] = input;
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
            
            return Equals((OneFrameInputs) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Inputs);
        }

        public static bool operator==(OneFrameInputs a, OneFrameInputs b)
        {
            if (a is null || b is null)
            {
                if (a is null && b is null)
                {
                    return true;
                }
                return false;
            }
            
            if (a.Inputs.Count != b.Inputs.Count)
            {
                return false;
            }

            foreach (var kv in a.Inputs)
            {
                if (!b.Inputs.TryGetValue(kv.Key, out LSInput inputInfo))
                {
                    return false;
                }

                if (kv.Value != inputInfo)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(OneFrameInputs a, OneFrameInputs b)
        {
            return !(a == b);
        }
    }
}