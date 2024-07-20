using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class CastConfigCategory : Singleton<CastConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, CastConfig> dict = new();
		
        public void Merge(object o)
        {
            CastConfigCategory s = o as CastConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public CastConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CastConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (CastConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CastConfig> GetAll()
        {
            return this.dict;
        }

        public CastConfig GetOne()
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

	public partial class CastConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>Name</summary>
		public string Name { get; set; }
		/// <summary>Action分组</summary>
		public int ActionGroup { get; set; }
		/// <summary>碰撞体种类</summary>
		public int RigidBody { get; set; }

	}
}
