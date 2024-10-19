namespace ET
{
    public class ActionHandlerAttribute : BaseAttribute
    {
        
    }
    
    [ActionHandler]
    public abstract class AActionHandler : HandlerObject
    {
        public abstract bool Check(ActionComponent actionComponent, ActionConfig config);
        
        public abstract void Update(ActionComponent actionComponent, ActionConfig config);
    }
}