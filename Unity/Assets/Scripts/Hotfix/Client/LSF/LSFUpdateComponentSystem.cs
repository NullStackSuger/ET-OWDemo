using System;
using System.Collections.Generic;
using System.IO;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFUpdateComponent))]
    [FriendOf(typeof(LSFUpdateComponent))]
    [FriendOf(typeof(ET.Room))]
    public static partial class LSFUpdateComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSFUpdateComponent self)
        {
        }
        public static void Init(this ET.Room self, long playerId, List<LockStepUnitInfo> unitInfos, long startTime, int frame = -1)
        {
            self.AddComponent<LSFTimerComponent>();
            
            self.AuthorityFrame = frame;
            self.PredictionFrame = frame;
            self.FrameBuffer = new(frame);
            self.FixedTimeCounter = new(startTime, 0, LSFConfig.NormalTickRate);
            self.PlayerId = playerId;
            foreach (var info in unitInfos)
            {
                self.PlayerIds.Add(info.PlayerId);
            }
            self.StartTime = startTime;
            self.IsReplay = self.Replay != null;
            if (!self.IsReplay)
            {
                self.Replay = new();
                self.Replay.UnitInfos = unitInfos;
            }

            self.PredictionWorld = new LSWorld(1, SceneType.LSFClientPrediction);
            LSUnitComponent unitComponent = self.PredictionWorld.AddComponent<LSUnitComponent>();
            foreach (var info in unitInfos)
            {
                unitComponent.Creat(info, Tag.PlayerA);
            }
            
            self.AuthorityWorld = new LSWorld(2, SceneType.LSFClientAuthority);
            unitComponent = self.AuthorityWorld.AddComponent<LSUnitComponent>();
            foreach (var info in unitInfos)
            {
                unitComponent.Creat(info, Tag.PlayerA);
            }
        }


        
        [EntitySystem]
        private static void Update(this LSFUpdateComponent self)
        {
            ET.Room room = self.GetParent<ET.Room>();
            long now = TimeInfo.Instance.ServerNow();

            while (true)
            {
                // 检查时间是否通过
                // 放循环里 比如一次Update是0.4s 一帧0.2s 那一次Update就要执行2帧
                long next = room.FixedTimeCounter.FrameTime(room.PredictionFrame + 1);
                if (now < next) return;

                // 最多预测5帧
                if (room.PredictionFrame - room.AuthorityFrame > 5) return;

                ++room.PredictionFrame;

                // 获取输入
                OneFrameInputs inputs = GetInputs(room, room.PredictionFrame);
                // 处理输入
                room.PredictionWorld.Update(inputs);
                // 广播输入
                FrameMessage frameMessage = FrameMessage.Create();
                frameMessage.Frame = room.PredictionFrame;
                frameMessage.Input = room.Input;
                room.Root().GetComponent<ClientSenderComponent>().Send(frameMessage);

                // 获取(保存)消息
                var deltaEvents = GetDeltaEvents(room, room.PredictionFrame);
                
                room.FrameBuffer.MoveForward(room.PredictionFrame);
                room.Input.Clear();
                room.DeltaEvents.Clear();

                // 单次update时间>5ms 就留到下次update再做
                // 避免单次update时间太长 卡住
                long now2 = TimeInfo.Instance.ServerNow();
                if (now2 > now + 5) break;
            }
        }
        public static void Update(this ET.LSWorld world, OneFrameInputs inputs)
        {
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            
            // 更新输入
            foreach (var pair in inputs.Inputs)
            {
                LSUnit unit = unitComponent.GetChild<LSUnit>(pair.Key);
                LSFInputComponent inputComponent = unit.GetComponent<LSFInputComponent>();
                inputComponent.Input = pair.Value;
            }
            
            // 调用World的UpdateSystem
            world.Update();
        }
        private static OneFrameInputs GetInputs(ET.Room room, int frame)
        {
            FrameBuffer frameBuffer = room.FrameBuffer;

            // 当前帧在权威帧之前, 消息可靠
            if (frame <= room.AuthorityFrame) return frameBuffer.FrameInputs(frame);

            // 预测
            OneFrameInputs predictionInputs = frameBuffer.FrameInputs(frame);
            
            // 其他人用权威帧输入, 自己预测输入
            // 没有权威帧输入的话 服务端会new
            if (frameBuffer.CheckFrame(room.AuthorityFrame))
            {
                OneFrameInputs authorityInputs = frameBuffer.FrameInputs(room.AuthorityFrame);
                
                //authorityInputs.CopyTo(predictionInputs);
                authorityInputs.CopyToPrediction(predictionInputs);
            }
            //predictionInputs.Inputs[room.PlayerId] = room.Input;
            predictionInputs.CopyEach(room.PlayerId, ref room.Input);
            
            predictionInputs.Frame = room.PredictionFrame;

            return predictionInputs;
        }
        private static OneFrameDeltaEvents GetDeltaEvents(ET.Room room, int frame)
        {
            FrameBuffer frameBuffer = room.FrameBuffer;

            // 当前帧在权威帧之前, 消息可靠
            if (frame <= room.AuthorityFrame) return frameBuffer.DeltaEvents(frame);

            // 当前帧是预测帧
            OneFrameDeltaEvents deltaEvents = frameBuffer.DeltaEvents(frame);
            deltaEvents.Clear();
            foreach (var kv1 in room.DeltaEvents)
            {
                foreach (var kv2 in kv1.Value)
                {
                    deltaEvents.Events.Add($"{kv1.Key}_{kv2.Key}", kv2.Value);
                }
            }
            deltaEvents.Frame = frame;

            return deltaEvents;
        }

        
        
        public static void Rollback(this ET.Room self)
        {
            // 更换世界
            self.PredictionWorld.Dispose();
            LSWorld lsWorld = self.AuthorityWorld.Clone();
            lsWorld.Id = 1;
            lsWorld.SceneType = SceneType.RollBack;
            self.PredictionWorld = lsWorld;

            // 获取输入
            FrameBuffer frameBuffer = self.FrameBuffer;
            OneFrameInputs authorityInputs = frameBuffer.FrameInputs(self.AuthorityFrame);

            // 先Update一次回到权威帧结束时
            // 因为SaveWorld发生在Update之前
            self.PredictionWorld.Update(authorityInputs);

            // 重新预测
            for (int i = self.AuthorityFrame + 1; i <= self.PredictionFrame; ++i)
            {
                OneFrameInputs inputs = frameBuffer.FrameInputs(i);
                authorityInputs.CopyTo(inputs, self.PlayerId);
                self.PredictionWorld.Update(inputs);
            }

            // RollbackSystem
            RollbackSystem(self);

            self.PredictionWorld.SceneType = SceneType.LSFClientPrediction;

            void RollbackSystem(Entity entity)
            {
                // 因为LSEntity全在World下面, 回滚时是拷贝World, 如果LSEntity需要回滚 可能是设计有问题了
                if (entity is LSEntity) return;

                LSEntitySystemSingleton.Instance.LSRollback(entity);

                foreach (var kv in entity.Components)
                {
                    RollbackSystem(kv.Value);
                }

                foreach (var kv in entity.Children)
                {
                    RollbackSystem(kv.Value);
                }
            }
        }
        
        

        public static void SaveReplay(this ET.Room self, string path)
        {
            if (self.IsReplay) return;
            Log.Debug($"save replay: {path} frame: {self.Replay.DeltaEvents.Count}");
            byte[] bytes = MemoryPackHelper.Serialize(self.Replay);
            Log.Warning($"Save Path {path}");
            File.WriteAllBytes(path, bytes);
        }
        public static void Record(this ET.Room self, int frame)
        {
            if (frame > self.AuthorityFrame) return;
            
            // 即使当前帧没有消息发过来也要去Record, 不然消息数和帧数对不上
            self.Record(frame, "", null);

            // 每分钟记录下快照
            if (frame % LSFConfig.FrameCountPerMinute == 0)
            {
                // 生成World的快照和Hash
                MemoryBuffer memoryBuffer = new(1024);
                memoryBuffer.Seek(0, SeekOrigin.Begin);
                memoryBuffer.SetLength(0);
                MemoryPackHelper.Serialize(self.AuthorityWorld, memoryBuffer);
                memoryBuffer.Seek(0, SeekOrigin.Begin);
                long hash = memoryBuffer.GetBuffer().Hash(0, (int)memoryBuffer.Length);
                self.FrameBuffer.SetHash(self.AuthorityFrame, hash);
                // 保存到回放文件
                byte[] bytes = memoryBuffer.ToArray();
                self.Replay.Snapshots.Add(bytes);
            }
        }
        public static void Record(this ET.Room self, int frame, string key, MessageObject value)
        {
            if (frame > self.AuthorityFrame) return;
            if (frame <= -1) return;
            
            // 每帧记录下输入
            if (self.Replay.DeltaEvents.Count < frame + 1)
            {
                OneFrameDeltaEvents saveDelta = OneFrameDeltaEvents.Create();
                saveDelta.Frame = frame;
                self.Replay.DeltaEvents.Add(saveDelta);
            }

            if (self.Replay.DeltaEvents.Count != frame + 1)
            {
                Log.Error($"回放第{frame}帧出现问题 : {self.Replay.DeltaEvents.Count}");
            }
            
            if (value == null) return;

            OneFrameDeltaEvents events = self.Replay.DeltaEvents[frame];
            events.Events.Add(key, value);
        }
    }
}