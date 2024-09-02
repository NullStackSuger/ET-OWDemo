using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class AnimatorConfigCategory : Singleton<AnimatorConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, AnimatorConfig> dict = new();
		
        public void Merge(object o)
        {
            AnimatorConfigCategory s = o as AnimatorConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public AnimatorConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AnimatorConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AnimatorConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AnimatorConfig> GetAll()
        {
            return this.dict;
        }

        public AnimatorConfig GetOne()
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

	public partial class AnimatorConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>Name</summary>
		public string Name { get; set; }
		/// <summary>Tag</summary>
		public string Tag { get; set; }

	}
}
