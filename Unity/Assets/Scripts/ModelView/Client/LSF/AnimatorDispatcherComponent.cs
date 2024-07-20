using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Client
{
    [Code]
    public class AnimatorDispatcherComponent : Singleton<AnimatorDispatcherComponent>, ISingletonAwake
    {
        private readonly Dictionary<AnimatorType, AAnimatorHandler> Handlers = new();
        
        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof (AnimatorHandlerAttribute));
            foreach (Type type in types)
            {
                AAnimatorHandler handler = Activator.CreateInstance(type) as AAnimatorHandler;
                if (handler == null)
                {
                    Log.Error($"{nameof(AAnimatorHandler)} is Null: {type.Name}");
                    continue;
                }

                AnimatorHandlerAttribute attribute = type.GetCustomAttributes(typeof(AnimatorHandlerAttribute), false)[0] as AnimatorHandlerAttribute;
                this.Handlers.Add(attribute.Type, handler);
            }
        }
        
        public AAnimatorHandler this [AnimatorType key]
        {
            get
            {
                return Handlers[key];
            }
        }
    }
}