using System;
using System.Collections.Generic;

namespace ET
{
    [Code]
    public class ActionDispatcherComponent : Singleton<ActionDispatcherComponent>, ISingletonAwake
    {
        private readonly Dictionary<string, AActionHandler> Handlers = new();
        
        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof (ActionHandlerAttribute));
            foreach (Type type in types)
            {
                AActionHandler handler = Activator.CreateInstance(type) as AActionHandler;
                if (handler == null)
                {
                    Log.Error($"{nameof(AActionHandler)} is Null: {type.Name}");
                    continue;
                }
                this.Handlers.Add(type.Name, handler);
            }
        }
        
        public AActionHandler this [string key]
        {
            get
            {
                return Handlers[key];
            }
        }
    }
}