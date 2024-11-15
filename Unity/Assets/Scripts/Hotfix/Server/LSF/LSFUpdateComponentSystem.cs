using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(LSFUpdateComponent))]
    [FriendOfAttribute(typeof(B3WorldComponent))]
    [FriendOfAttribute(typeof(ET.Room))]
    public static partial class LSFUpdateComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSFUpdateComponent self)
        {
        }
        public static void Init(this Room self, List<LockStepUnitInfo> unitInfos, long startTime, int frame = -1)
        {
            self.AddComponent<LSFTimerComponent>();
            self.AddComponent<AOIManagerComponent>();

            self.AuthorityFrame = frame;
            self.FrameBuffer = new(frame);
            self.FixedTimeCounter = new(startTime, 0, LSFConfig.NormalTickRate);
            foreach (var info in unitInfos)
            {
                self.PlayerIds.Add(info.PlayerId);
            }
            self.StartTime = startTime;
            
            self.AuthorityWorld = new LSWorld(SceneType.LSFServer);
            LSWorld world = self.AuthorityWorld;
            world.AddComponent<B3WorldComponent>();
            LSUnitComponent unitComponent = world.AddComponent<LSUnitComponent>();
            foreach (var info in unitInfos)
            {
                unitComponent.Creat(info, Tag.PlayerA);
            }
        }


        [EntitySystem]
        private static void Update(this LSFUpdateComponent self)
        {
            Room room = self.GetParent<Room>();

            // 检查当前时间是否能进行下一帧
            long now = TimeInfo.Instance.ServerFrameTime();
            long next = room.FixedTimeCounter.FrameTime(room.AuthorityFrame + 1);
            if (now < next) return;

            ++room.AuthorityFrame;

            // 获取输入
            OneFrameInputs inputs = GetInputs(room, room.AuthorityFrame);
            // 广播输入
            OneFrameInputs sendInputs = OneFrameInputs.Create();
            inputs.CopyTo(sendInputs);
            sendInputs.Frame = room.AuthorityFrame;
            room.BroadCast(sendInputs);
            // 处理输入
            room.Update(inputs);
            
            // 获取消息
            OneFrameDeltaEvents deltaEvents = GetDeltaEvents(room, room.AuthorityFrame);
            // 广播消息
            OneFrameDeltaEvents sendDeltaEvents = OneFrameDeltaEvents.Create();
            deltaEvents.CopyTo(sendDeltaEvents);
            room.BroadCast(deltaEvents);

            room.FrameBuffer.MoveForward(room.AuthorityFrame);
            room.DeltaEvents.Clear();
        }

        public static void Update(this ET.Room self, OneFrameInputs inputs)
        {
            LSWorld world = self.AuthorityWorld;
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
        
        private static OneFrameInputs GetInputs(Room room, int frame)
        {
            FrameBuffer frameBuffer = room.FrameBuffer;
            OneFrameInputs inputs = frameBuffer.FrameInputs(frame);
            frameBuffer.MoveForward(frame);

            // 输入数 == 匹配玩家数 正常
            if (inputs.Inputs.Count == LSFConfig.MatchCount) return inputs;

            // 输入数 != 匹配玩家数 缺的用上帧的补上
            OneFrameInputs lastInputs = null;
            if (frameBuffer.CheckFrame(frame - 1))
            {
                lastInputs = frameBuffer.FrameInputs(frame - 1);
            }
            // 有人输入的消息没过来，给他使用上一帧的操作
            foreach (long id in room.PlayerIds)
            {
                // 该玩家不缺输入
                if (inputs.Inputs.ContainsKey(id)) continue;

                // 有上帧输入且上帧输入里存在该玩家
                if (lastInputs != null && lastInputs.Inputs.TryGetValue(id, out LSInput input))
                {
                    inputs.Inputs[id] = input;
                }
                else
                {
                    inputs.Inputs[id] = new();
                }
            }
            
            inputs.Frame = frame;

            return inputs;
        }

        private static OneFrameDeltaEvents GetDeltaEvents(Room room, int frame)
        {
            FrameBuffer frameBuffer = room.FrameBuffer;

            OneFrameDeltaEvents deltaEvents = frameBuffer.DeltaEvents(frame);
            deltaEvents.Clear();
            foreach (var kv1 in room.DeltaEvents)
            {
                foreach (var kv2 in kv1.Value)
                {
                    deltaEvents.Add(kv2.Value.ToString(), kv2.Value);
                }
            }
            deltaEvents.Frame = frame;

            return deltaEvents;
        }
    }
}