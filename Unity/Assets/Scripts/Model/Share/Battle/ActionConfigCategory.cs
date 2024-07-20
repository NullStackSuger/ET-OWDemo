using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{

    public partial class ActionConfigCategory
    {
        [BsonIgnore]
        private readonly Dictionary<int, SortedDictionary<int, ActionConfig>> Groups = new();
        
        public SortedDictionary<int, ActionConfig> this[int groupIndex]
        {
            get
            {
                return this.Groups[groupIndex];
            }
        }

        public override void EndInit()
        {
            foreach (var pair in this.GetAll())
            {
                SortedDictionary<int, ActionConfig> group;
                if (!this.Groups.TryGetValue(pair.Value.Group, out group))
                {
                    group = new();
                    this.Groups.Add(pair.Value.Group, group);
                }
                
                group.Add(pair.Key, pair.Value);
            }
        }
    }
}