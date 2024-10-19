using System;
using System.Collections.Generic;

namespace ET.Server
{
    [Code]
    public class CollisionCallbackDispatcherComponent : Singleton<CollisionCallbackDispatcherComponent>, ISingletonAwake
    {
        private readonly Dictionary<string, ACollisionCallback> CollisionHandlers = new();
        
        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof (CollisionCallbackAttribute));
            foreach (Type type in types)
            {
                ACollisionCallback handler = Activator.CreateInstance(type) as ACollisionCallback;
                if (handler == null)
                {
                    Log.Error($"{nameof(ACollisionCallback)} is Null: {type.Name}");
                    continue;
                }
                this.CollisionHandlers.Add(type.Name, handler);
            }
        }
        
        public ACollisionCallback this[string key]
        {
            get
            {
                return this.CollisionHandlers.ContainsKey(key) ? this.CollisionHandlers[key] : null;
            }
        }
    }
}