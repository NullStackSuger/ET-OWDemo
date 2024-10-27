using System.Collections.Generic;

namespace ET
{
    public partial class AnimatorStateConfigCategory
    {
        private Dictionary<string, AnimatorStateConfig> Configs = new();
        
        public override void EndInit()
        {
            base.EndInit();

            foreach (var kv in this.GetAll())
            {
                this.Configs.Add(kv.Value.Name, kv.Value);
            }
        }

        public AnimatorStateConfig GetByName(string stateName)
        {
            return this.Configs[stateName];
        }
    }
}