/*using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class Room : Entity, IScene, IAwake<string>
    {
        public Fiber Fiber { get; set; }
        public SceneType SceneType { get; set; } = SceneType.Room;
        
        /// <summary>
        /// 需要和Scene Asset命名一致
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 帧缓存
        /// </summary>
        public FrameBuffer FrameBuffer { get; set; }
        
        /// <summary>
        /// FixedUpdate计时器
        /// </summary>
        public FixedTimeCounter FixedTimeCounter { get; set; }
        
        /// <summary>
        /// 玩家id列表
        /// Key: Id, Value: IsOnline
        /// </summary>
        public List<long> Players { get; } = new();

        /// <summary>
        /// 当前帧(权威帧)
        /// </summary>
        public int Frame { get; set; } = -1;

        private EntityRef<LSWorld> world;

        public LSWorld World
        {
            get
            {
                return this.world;
            }
            set
            {
                this.AddChild(value);
                this.world = value;
            }
        }
    }
}*/