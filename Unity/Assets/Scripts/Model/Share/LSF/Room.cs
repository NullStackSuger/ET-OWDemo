using System;
using System.Collections.Generic;

namespace ET
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
        /// 计算fixedTime，fixedTime在客户端是动态调整的，会做时间膨胀缩放
        /// </summary>
        public FixedTimeCounter FixedTimeCounter { get; set; }
        
        public long PlayerId { get; set; }
        /// <summary>
        /// 房间中所有玩家
        /// </summary>
        public List<long> PlayerIds { get; } = new(LSFConfig.MatchCount);
        
        /// <summary>
        /// 预测帧
        /// </summary>
        public int PredictionFrame { get; set; } = -1;
        /// <summary>
        /// 权威帧
        /// </summary>
        public int AuthorityFrame { get; set; } = -1;
        
        private EntityRef<LSWorld> predictionWorld;
        private EntityRef<LSWorld> authorityWorld;
        /// <summary>
        /// 预测World
        /// </summary>
        public LSWorld PredictionWorld
        {
            get
            {
                return this.predictionWorld;
            }
            set
            {
                this.AddChild(value);
                this.predictionWorld = value;
            }
        }
        /// <summary>
        /// 权威World
        /// </summary>
        public LSWorld AuthorityWorld
        {
            get
            {
                return this.authorityWorld;
            }
            set
            {
                this.AddChild(value);
                this.authorityWorld = value;
            }
        }
        
        public LSInput Input = new();
        public MultiDictionary<long, Type, MessageObject> DeltaEvents = new();
        
        public bool IsReplay { get; set; }
        public Replay Replay { get; set; }

        /// <summary>
        /// 为了断线重连而添加的
        /// </summary>
        public long StartTime { get; set; }
    }
}