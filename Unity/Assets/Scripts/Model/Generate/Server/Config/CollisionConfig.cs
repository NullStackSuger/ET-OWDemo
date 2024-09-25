using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class CollisionConfigCategory : Singleton<CollisionConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, CollisionConfig> dict = new();
		
        public void Merge(object o)
        {
            CollisionConfigCategory s = o as CollisionConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public CollisionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CollisionConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (CollisionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CollisionConfig> GetAll()
        {
            return this.dict;
        }

        public CollisionConfig GetOne()
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

	public partial class CollisionConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>IsTrigger</summary>
		public int IsTrigger { get; set; }
		/// <summary>逆转张量</summary>
		public string InvInertiaDiagLocal { get; set; }
		/// <summary>弹性系数</summary>
		public float Restitution { get; set; }
		/// <summary>ApplyHeadRotation</summary>
		public int ApplyHeadRotation { get; set; }
		/// <summary>Callback</summary>
		public string Callback { get; set; }

	}
}
