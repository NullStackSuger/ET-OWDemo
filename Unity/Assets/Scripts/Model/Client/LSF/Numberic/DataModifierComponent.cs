using System.Collections.Generic;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using UnityEngine;

namespace ET.Client
{
    public static partial class DataModifierType
    {
        public const int Speed = 1000;

        public const int Hp = 3000;
        
        public const int BulletCount = 4000;

        [StaticField]
        private static readonly List<int> Types = new();
        static DataModifierType()
        {
            foreach (FieldInfo info in typeof(DataModifierType).GetFields())
            {
                if (info.FieldType != typeof(int)) continue;

                int val = (int)info.GetValue(null);
                Types.Add(val);
            }
        }
        public static bool Contains(int item)
        {
            return Types.Contains(item);
        }
    }

    public struct DataModifierChange
    {
        public LSUnit Unit;
        public int DataModifierType;
        public long Old;
        public long New;
    }
    
    [ComponentOf(typeof(LSUnit))]
    public class DataModifierComponent : LSEntity, IAwake
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> NumericDic = new Dictionary<int, long>();
        
        public long this[int numericType]
        {
            get
            {
                return NumericDic[numericType];
            }
            set
            {
                long old;
                if (this.NumericDic.ContainsKey(numericType))
                {
                    old = NumericDic[numericType];
                    NumericDic[numericType] = value;
                }
                else
                {
                    old = 0;
                    NumericDic.Add(numericType, value);
                }

                EventSystem.Instance.Publish(this.IScene as LSWorld,
                    new DataModifierChange() { Unit = this.GetParent<LSUnit>(), DataModifierType = numericType, Old = old, New = value });
            }
        }
    }
}