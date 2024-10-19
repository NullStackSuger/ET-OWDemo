using System.Collections.Generic;
using System.IO;

namespace ET.Server
{
    public class CollisionDispatcherComponent : Singleton<CollisionDispatcherComponent>, ISingletonAwake
    {
        private readonly Dictionary<int, CollisionInfo> Infos = new();
        
        public void Awake()
        {
            string path = $"D:\\ColliderInfos.bytes";
            if (!File.Exists(path)) return;
            
            byte[] bytes = File.ReadAllBytes(path);
            List<CollisionInfo> infos = MemoryPackHelper.Deserialize(typeof(List<CollisionInfo>), bytes, 0, bytes.Length) as List<CollisionInfo>;

            foreach (CollisionInfo info in infos)
            {
                this.Infos.Add(info.Id, info);
            }
        }

        public CollisionInfo this[int index]
        {
            get
            {
                return this.Infos[index];
            }
        }
    }
}