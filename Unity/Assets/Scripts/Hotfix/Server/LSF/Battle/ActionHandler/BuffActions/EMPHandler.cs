using System.Text.RegularExpressions;

namespace ET.Server
{
    public class EMPInitHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            
        }
    }
    [FriendOf(typeof(ActionComponent))]
    public class EMPHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            ActionComponent actions = actionComponent.GetParent<Buff>().GetParent<BuffComponent>().GetParent<LSUnit>().GetComponent<ActionComponent>();
            foreach (ActionConfig actionConfig in actions.Configs)
            {
                if (actions.Actives[actionConfig] == false) continue;
                
                string pattern = @".+?Cast[a-zA-Z]Handler$";
                if (!Regex.IsMatch(actionConfig.Name, pattern)) continue;

                actions.Actives[actionConfig] = false;
            }
        }
    }
    [FriendOf(typeof(ActionComponent))]
    public class EMPEndHandler : AActionHandler
    {
        public override bool Check(ActionComponent actionComponent, ActionConfig config)
        {
            return true;
        }

        public override void Update(ActionComponent actionComponent, ActionConfig config)
        {
            ActionComponent actions = actionComponent.GetParent<Buff>().GetParent<BuffComponent>().GetParent<LSUnit>().GetComponent<ActionComponent>();
            foreach (ActionConfig actionConfig in actions.Configs)
            {
                if (!actions.Actives[actionConfig] == true) continue;
                
                string pattern = @".*?Cast.Handler";
                if (!Regex.IsMatch(actionConfig.Name, pattern)) continue;

                actions.Actives[actionConfig] = true;
            }
        }
    }
}