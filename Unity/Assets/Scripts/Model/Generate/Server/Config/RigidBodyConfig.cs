using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class RigidBodyConfigCategory : Singleton<RigidBodyConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, RigidBodyConfig> dict = new();
		
        public void Merge(object o)
        {
            RigidBodyConfigCategory s = o as RigidBodyConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public RigidBodyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RigidBodyConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (RigidBodyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RigidBodyConfig> GetAll()
        {
            return this.dict;
        }

        public RigidBodyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            
            var enumerator = this.dict.Values.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current; 
        }
    }

	public partial class RigidBodyConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>Mass</summary>
		public float Mass { get; set; }
		/// <summary>PosX</summary>
		public float X { get; set; }
		/// <summary>PosY</summary>
		public float Y { get; set; }
		/// <summary>PosZ</summary>
		public float Z { get; set; }
		/// <summary>Shape</summary>
		public string Shape { get; set; }
		/// <summary>Args</summary>
		public long[] Args { get; set; }

	}
}
