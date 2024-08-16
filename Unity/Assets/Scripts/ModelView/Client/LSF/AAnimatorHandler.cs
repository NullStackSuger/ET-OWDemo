namespace ET.Client
{
    public class AnimatorHandlerAttribute : BaseAttribute
    {
        public AnimatorType Type;

        public AnimatorHandlerAttribute(AnimatorType type)
        {
            this.Type = type;
        }
    } 
    
    
    public abstract class AAnimatorHandler : HandlerObject
    {
        public abstract void Update(LSFAnimatorComponent animatorComponent, LSUnit unit);
    }
}