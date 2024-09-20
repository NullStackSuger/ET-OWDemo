namespace ET.Client
{
    public class AnimatorHandlerAttribute : BaseAttribute
    {
        public string Type;

        public AnimatorHandlerAttribute(string type)
        {
            this.Type = type;
        }
    } 
    
    
    public abstract class AAnimatorHandler : HandlerObject
    {
        public abstract void Update(LSFAnimatorComponent animatorComponent, LSUnit unit);
    }
}