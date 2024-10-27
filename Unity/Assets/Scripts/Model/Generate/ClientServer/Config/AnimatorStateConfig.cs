using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class AnimatorStateConfigCategory : Singleton<AnimatorStateConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, AnimatorStateConfig> dict = new();
		
        public void Merge(object o)
        {
            AnimatorStateConfigCategory s = o as AnimatorStateConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public AnimatorStateConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AnimatorStateConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AnimatorStateConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AnimatorStateConfig> GetAll()
        {
            return this.dict;
        }

        public AnimatorStateConfig GetOne()
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

	public partial class AnimatorStateConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>StateName</summary>
		public string Name { get; set; }
		/// <summary>Layer</summary>
		public string Layer { get; set; }
		/// <summary>淡入淡出时间</summary>
		public float FadeDuration { get; set; }
		/// <summary>能被优先级更低的打断</summary>
		public int BreakUnderAble { get; set; }
		/// <summary>结束时切换到哪个动画</summary>
		public string PlayOnEnd { get; set; }

	}
}
