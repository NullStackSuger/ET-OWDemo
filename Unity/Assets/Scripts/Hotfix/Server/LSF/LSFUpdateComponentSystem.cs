using System.Collections.Generic;
using System.Linq;
using BulletSharp;
using BulletSharp.Math;
using MongoDB.Bson;

namespace ET.Server
{
    [EntitySystemOf(typeof(LSFUpdateComponent))]
    [FriendOfAttribute(typeof(ET.B3WorldComponent))]
    public static partial class LSFUpdateComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSFUpdateComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this LSFUpdateComponent self)
        {
            ET.Room room = self.GetParent<ET.Room>();

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
            // Server To Client 是状态同步, 直接在LSUpdate里RoomMessageHelper.BroadCast
            room.Update(inputs);
        }

        private static OneFrameInputs GetInputs(ET.Room room, int frame)
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

            return inputs;
        }

        public static void Init(this ET.Room self, List<LockStepUnitInfo> unitInfos, long startTime, int frame = -1)
        {
            self.AuthorityFrame = frame;
            self.FrameBuffer = new(frame);
            self.FixedTimeCounter = new(startTime, 0, LSFConfig.NormalTickRate);

            self.AuthorityWorld = new LSWorld(SceneType.LSFServer);
            LSWorld world = self.AuthorityWorld;
            world.AddComponent<B3WorldComponent>();
            LSUnitComponent unitComponent = world.AddComponent<LSUnitComponent>();

            foreach (var info in unitInfos)
            {
                LSUnit unit = unitComponent.Creat(info, TeamTag.TeamA);
                unit.AddComponent<LSFInputComponent>();
                unit.AddComponent<B3CollisionComponent, int>(5);
                DataModifierComponent dataModifierComponent = unit.AddComponent<DataModifierComponent>();
                dataModifierComponent.Add(new Default_Speed_ConstantModifier() { Value = 100 });
                dataModifierComponent.Add(new Default_Hp_FinalMaxModifier() { Value = 100 });
                dataModifierComponent.Add(new Default_Hp_FinalMinModifier() { Value = 0 });
                dataModifierComponent.Add(new Default_Hp_ConstantModifier() { Value = 10 });
                dataModifierComponent.Add(new Default_Hp_FinalConstantModifier() { Value = 10 });
                //dataModifierComponent.Add(new Default_MaxAtk_Modifier(10));
                //dataModifierComponent.Add(new Default_MinAtk_Modifier(0));
                dataModifierComponent.Add(new Default_Atk_ConstantModifier() { Value = 5 });
                
                self.PlayerIds.Add(info.PlayerId);
            }

            self.StartTime = startTime;
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
    }
}