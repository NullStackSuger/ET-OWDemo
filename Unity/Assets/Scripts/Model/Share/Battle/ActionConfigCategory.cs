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
                int groupId = pair.Key / 1000;
                if (!this.Groups.TryGetValue(groupId, out group))
                {
                    group = new();
                    this.Groups.Add(groupId, group);
                }
                
                group.Add(pair.Key, pair.Value);
            }
        }
    }
}