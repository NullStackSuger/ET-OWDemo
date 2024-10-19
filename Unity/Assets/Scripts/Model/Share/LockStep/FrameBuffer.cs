using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{
    public class FrameBuffer: Object
    {
        public int MaxFrame { get; private set; }
        private readonly List<OneFrameInputs> frameInputs; // 每帧输入, 客户端是自己和别人的预测输入, 服务端是所有玩家输入
        private readonly List<OneFrameDeltaEvents> deltaEvents; // 每帧的事件, 只有权威World和服务器会用, 所以可以放在这里
        private readonly List<MemoryBuffer> snapshots; // 每帧快照
        private readonly List<long> hashs; // 每帧快照的哈希值
        

        public FrameBuffer(int frame = 0, int capacity = LSConstValue.FrameCountPerSecond * 60)
        {
            this.MaxFrame = frame + LSConstValue.FrameCountPerSecond * 30;
            this.frameInputs = new List<OneFrameInputs>(capacity);
            this.deltaEvents = new List<OneFrameDeltaEvents>(capacity);
            this.snapshots = new List<MemoryBuffer>(capacity);
            this.hashs = new List<long>(capacity);
            
            for (int i = 0; i < capacity; ++i)
            {
                this.hashs.Add(0);
                this.frameInputs.Add(OneFrameInputs.Create());
                this.deltaEvents.Add(OneFrameDeltaEvents.Create());
                MemoryBuffer memoryBuffer = new(10240);
                memoryBuffer.SetLength(0);
                memoryBuffer.Seek(0, SeekOrigin.Begin);
                this.snapshots.Add(memoryBuffer);
            }
        }

        public void SetHash(int frame, long hash)
        {
            EnsureFrame(frame);
            this.hashs[frame % this.frameInputs.Capacity] = hash;
        }
        
        public long GetHash(int frame)
        {
            EnsureFrame(frame);
            return this.hashs[frame % this.frameInputs.Capacity];
        }

        public bool CheckFrame(int frame)
        {
            if (frame < 0)
            {
                return false;
            }

            if (frame > this.MaxFrame)
            {
                return false;
            }

            return true;
        }

        private void EnsureFrame(int frame)
        {
            if (!CheckFrame(frame))
            {
                throw new Exception($"frame out: {frame}, maxframe: {this.MaxFrame}");
            }
        }
        
        public void MoveForward(int frame)
        {
            if (this.MaxFrame - frame > LSFConfig.FrameCountPerSecond) // 至少留出1秒的空间
            {
                return;
            }
            
            ++this.MaxFrame;
            
            OneFrameInputs oneFrameInputs = this.FrameInputs(this.MaxFrame);
            oneFrameInputs.Inputs.Clear();
        }
        
        public OneFrameInputs FrameInputs(int frame)
        {
            EnsureFrame(frame);
            OneFrameInputs oneFrameInputs = this.frameInputs[frame % this.frameInputs.Capacity];
            return oneFrameInputs;
        }

        public OneFrameDeltaEvents DeltaEvents(int frame)
        {
            EnsureFrame(frame);
            OneFrameDeltaEvents oneFrameDeltaEvents = this.deltaEvents[frame % this.deltaEvents.Capacity];
            return oneFrameDeltaEvents;
        }

        /// <summary>
        /// 获取对应帧快照
        /// </summary>
        public MemoryBuffer Snapshot(int frame)
        {
            EnsureFrame(frame);
            MemoryBuffer memoryBuffer = this.snapshots[frame % this.snapshots.Capacity];
            return memoryBuffer;
        }
    }
}