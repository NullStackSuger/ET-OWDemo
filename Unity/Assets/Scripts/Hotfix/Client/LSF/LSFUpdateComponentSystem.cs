using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using TrueSync;

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

        [EntitySystem]
        private static void Destroy(this LSFUpdateComponent self)
        {
            Room room = self.GetParent<Room>();
            //if (!room.IsReplay) room.SaveReplay("D:\\RP.replay");
        }

        [EntitySystem]
        private static void Update(this LSFUpdateComponent self)
        {
            ET.Room room = self.GetParent<ET.Room>();
            Scene root = room.Root();
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

                OneFrameInputs inputs = GetInputs(room, room.PredictionFrame);
                room.PredictionWorld.Update(inputs);
                //room.SendHash(room.Frame);
                
                FrameMessage frameMessage = FrameMessage.Create();
                frameMessage.Frame = room.PredictionFrame;
                frameMessage.Input = room.Input;
                root.GetComponent<ClientSenderComponent>().Send(frameMessage);
                Log.Warning($"发送预测消息:{frameMessage.Frame}, {frameMessage.Input.Look}");

                room.Input.Clear();

                // 单次update时间>5ms 就留到下次update再做
                // 避免单次update时间太长 卡住
                long now2 = TimeInfo.Instance.ServerNow();
                if (now2 > now + 5) break;
            }
        }

        private static OneFrameInputs GetInputs(ET.Room room, int frame)
        {
            FrameBuffer frameBuffer = room.FrameBuffer;

            // 当前帧在权威帧之前, 消息可靠
            if (frame <= room.AuthorityFrame) return frameBuffer.FrameInputs(frame);

            // 预测
            OneFrameInputs predictionInputs = frameBuffer.FrameInputs(frame);
            frameBuffer.MoveForward(frame);
            // 其他人用权威帧输入, 自己预测输入
            // 没有权威帧输入的话 服务端会new
            if (frameBuffer.CheckFrame(room.AuthorityFrame))
            {
                OneFrameInputs authorityInputs = frameBuffer.FrameInputs(room.AuthorityFrame);
                authorityInputs.CopyToPrediction(predictionInputs);
            }
            predictionInputs.CopyEach(room.PlayerId, ref room.Input);

            return predictionInputs;
        }

        public static void Init(this ET.Room self, long playerId, List<LockStepUnitInfo> unitInfos, long startTime, int frame = -1)
        {
            self.AddComponent<LSFTimerComponent>();
            
            self.AuthorityFrame = frame;
            self.PredictionFrame = frame;
            self.FrameBuffer = new(frame);
            self.FixedTimeCounter = new(startTime, 0, LSFConfig.NormalTickRate);

            self.PlayerId = playerId;

            self.PredictionWorld = new LSWorld(1, SceneType.LSFClientPrediction);
            self.PredictionWorld.AddComponent<B3WorldComponent>();
            LSUnitComponent unitComponent = self.PredictionWorld.AddComponent<LSUnitComponent>();
            
            foreach (var info in unitInfos)
            {
                LSUnit unit = unitComponent.Creat(info, Tag.PlayerA);
                unit.AddComponent<LSFInputComponent>();
                unit.AddComponent<B3CollisionComponent, int>(5);
                unit.AddComponent<CheckOnGroundComponent>();
                
                DataModifierComponent dataModifierComponent = unit.AddComponent<DataModifierComponent>();
                
                dataModifierComponent.Add(new Default_Speed_ConstantModifier() { Value = 15 });
                
                dataModifierComponent.Add(new Default_Hp_FinalMaxModifier() { Value = 100 });
                dataModifierComponent.Add(new Default_Hp_FinalMinModifier() { Value = 0 });
                
                dataModifierComponent.Add(new Default_Hp_ConstantMaxModifier() { Value = 50 });
                dataModifierComponent.Add(new Default_Hp_ConstantMinModifier() { Value = 0 });
                dataModifierComponent.Add(new Default_Hp_ConstantModifier() { Value = 50 });
                
                dataModifierComponent.Add(new Default_Hp_FinalConstantMaxModifier() { Value = 50 });
                dataModifierComponent.Add(new Default_Hp_FinalConstantMinModifier() { Value = 0 });
                dataModifierComponent.Add(new Default_Hp_FinalConstantModifier() { Value = 50 });
                
                dataModifierComponent.Add(new Default_Atk_ConstantModifier() { Value = 5 });
                
                dataModifierComponent.Add(new Default_BulletCount_FinalMaxModifier() { Value = 150 });
                dataModifierComponent.Add(new Default_BulletCount_FinalMinModifier() { Value = 0 });
                dataModifierComponent.Add(new Default_BulletCount_ConstantModifier() { Value = 150 });
                
                dataModifierComponent.Add(new Default_Shield_FinalMaxModifier() { Value = 200 });
                dataModifierComponent.Add(new Default_Shield_FinalMinModifier() { Value = 0 });
                dataModifierComponent.Add(new Default_Shield_ConstantModifier() { Value = 200 });

                dataModifierComponent.Add(new Default_ENumeric_ConstantModifier() { Value = 30 });
                dataModifierComponent.Add(new Default_ECD_ConstantModifier() { Value = 1000 });
                
                dataModifierComponent.Add(new Default_QNumeric_ConstantModifier() { Value = 30 });
                dataModifierComponent.Add(new Default_QCD_ConstantModifier() { Value = 1000 });
                
                dataModifierComponent.Add(new Default_CNumeric_ConstantModifier() { Value = 30 });
                dataModifierComponent.Add(new Default_CCD_ConstantModifier() { Value = 1000 });
                
                dataModifierComponent.Publish(DataModifierType.Hp);
                dataModifierComponent.Publish(DataModifierType.BulletCount);
                
                self.PlayerIds.Add(info.PlayerId);
            }
            
            self.IsReplay = self.Replay != null;
            if (!self.IsReplay)
            {
                self.Replay = new();
                self.Replay.UnitInfos = unitInfos;

                self.AuthorityWorld = new LSWorld(2, SceneType.LSFClientAuthority);
                self.AuthorityWorld.AddComponent<B3WorldComponent>();
                unitComponent = self.AuthorityWorld.AddComponent<LSUnitComponent>();
                foreach (var info in unitInfos)
                {
                    LSUnit unit = unitComponent.Creat(info, Tag.PlayerA);
                    unit.AddComponent<LSFInputComponent>();
                    unit.AddComponent<B3CollisionComponent, int>(5);
                    unit.AddComponent<CheckOnGroundComponent>();
                    
                    DataModifierComponent dataModifierComponent = unit.AddComponent<DataModifierComponent>();
                    
                    dataModifierComponent.Add(new Default_Speed_ConstantModifier() { Value = 15 });
                    
                    dataModifierComponent.Add(new Default_Hp_FinalMaxModifier() { Value = 100 });
                    dataModifierComponent.Add(new Default_Hp_FinalMinModifier() { Value = 0 });
                
                    dataModifierComponent.Add(new Default_Hp_ConstantMaxModifier() { Value = 50 });
                    dataModifierComponent.Add(new Default_Hp_ConstantMinModifier() { Value = 0 });
                    dataModifierComponent.Add(new Default_Hp_ConstantModifier() { Value = 50 });
                
                    dataModifierComponent.Add(new Default_Hp_FinalConstantMaxModifier() { Value = 50 });
                    dataModifierComponent.Add(new Default_Hp_FinalConstantMinModifier() { Value = 0 });
                    dataModifierComponent.Add(new Default_Hp_FinalConstantModifier() { Value = 50 });
                    
                    dataModifierComponent.Add(new Default_Atk_ConstantModifier() { Value = 5 });
                    
                    dataModifierComponent.Add(new Default_BulletCount_FinalMaxModifier() { Value = 150 });
                    dataModifierComponent.Add(new Default_BulletCount_FinalMinModifier() { Value = 0 });
                    dataModifierComponent.Add(new Default_BulletCount_ConstantModifier() { Value = 150 });
                    
                    dataModifierComponent.Add(new Default_Shield_FinalMaxModifier() { Value = 200 });
                    dataModifierComponent.Add(new Default_Shield_FinalMinModifier() { Value = 0 });
                    dataModifierComponent.Add(new Default_Shield_ConstantModifier() { Value = 200 });
                    
                    dataModifierComponent.Add(new Default_ENumeric_ConstantModifier() { Value = 30 });
                    dataModifierComponent.Add(new Default_ECD_ConstantModifier() { Value = 1000 });
                    
                    dataModifierComponent.Add(new Default_QNumeric_ConstantModifier() { Value = 30 });
                    dataModifierComponent.Add(new Default_QCD_ConstantModifier() { Value = 1000 });
                    
                    dataModifierComponent.Add(new Default_CNumeric_ConstantModifier() { Value = 30 });
                    dataModifierComponent.Add(new Default_CCD_ConstantModifier() { Value = 1000 });
                    
                    dataModifierComponent.Publish(DataModifierType.Hp);
                    dataModifierComponent.Publish(DataModifierType.BulletCount);
                }
            }

            self.StartTime = startTime;
        }

        public static void Update(this ET.LSWorld world, OneFrameInputs inputs)
        {
            //LSWorld world = self.PredictionWorld;
            LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
            
            // 更新输入
            foreach (var pair in inputs.Inputs)
            {
                LSUnit unit = unitComponent.GetChild<LSUnit>(pair.Key);
                LSFInputComponent inputComponent = unit.GetComponent<LSFInputComponent>();
                inputComponent.Input = pair.Value;
            }
            
            //if (!self.IsReplay) self.SaveWorld(self.PredictionFrame);
            
            // 调用World的UpdateSystem
            world.Update();
        }
        
        public static void Rollback(this ET.Room self)
        {
            // 更换世界
            self.PredictionWorld.Dispose();
            self.PredictionWorld = self.AuthorityWorld.Clone();
            // TODO self.PredictionWorld.Id = 1;
            self.PredictionWorld.SceneType = SceneType.RollBack;

            // 获取输入
            FrameBuffer frameBuffer = self.FrameBuffer;
            OneFrameInputs authorityInputs = frameBuffer.FrameInputs(self.AuthorityFrame);

            // 先Update一次回到权威帧结束时
            // 因为SaveWorld发生在Update之前
            self.PredictionWorld.Update(authorityInputs);
            //self.SendHash(self.AuthorityFrame);

            // 重新预测
            for (int i = self.AuthorityFrame + 1; i <= self.PredictionFrame; ++i)
            {
                OneFrameInputs inputs = frameBuffer.FrameInputs(i);
                // TODO: ??? 这里为什么用最后一个权威帧输入Update
                authorityInputs.CopyTo(inputs, self.PlayerId);
                self.PredictionWorld.Update(inputs);
            }

            // RollbackSystem
            RollbackSystem(self);

            self.PredictionWorld.SceneType = SceneType.LSFClientPrediction;

            void RollbackSystem(Entity entity)
            {
                // TODO: ???
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

        private static LSWorld Clone(this LSWorld self)
        {
            using MemoryBuffer memoryBuffer = new(10240);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            memoryBuffer.SetLength(0);
            MemoryPackHelper.Serialize(self, memoryBuffer);
            
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof (LSWorld), memoryBuffer) as LSWorld;
            return lsWorld;
        }

        public static void SaveReplay(this ET.Room self, string path)
        {
            if (self.IsReplay) return;
            Log.Debug($"save replay: {path} frame: {self.Replay.FrameInputs.Count}");
            byte[] bytes = MemoryPackHelper.Serialize(self.Replay);
            Log.Warning($"Save Path {path}");
            File.WriteAllBytes(path, bytes);
        }

        public static void Record(this ET.Room self, int frame)
        {
            if (frame > self.AuthorityFrame) return;

            // 每帧记录下输入
            OneFrameInputs oneFrameInputs = self.FrameBuffer.FrameInputs(frame);
            OneFrameInputs saveInput = OneFrameInputs.Create();
            oneFrameInputs.CopyTo(saveInput);
            self.Replay.FrameInputs.Add(saveInput);

            // 每分钟记录下快照
            if (frame % LSConstValue.SaveLSWorldFrameCount == 0)
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
    }
}