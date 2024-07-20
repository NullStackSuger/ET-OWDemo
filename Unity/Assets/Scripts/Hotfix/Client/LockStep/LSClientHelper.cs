/*using System.IO;

namespace ET.Client
{
    public static partial class LSClientHelper
    {
        /// <summary>
        /// 调用所有RollBackSystem
        /// </summary>
        public static void RunLSRollbackSystem(Entity entity)
        {
            if (entity is LSEntity)
            {
                return;
            }
            
            LSEntitySystemSingleton.Instance.LSRollback(entity);
            
            if (entity.ComponentsCount() > 0)
            {
                foreach (var kv in entity.Components)
                {
                    RunLSRollbackSystem(kv.Value);
                }
            }

            if (entity.ChildrenCount() > 0)
            {
                foreach (var kv in entity.Children)
                {
                    RunLSRollbackSystem(kv.Value);
                }
            }
        }
        
        // 回滚
        public static void Rollback(ET.Room room, int frame)
        {
            room.LSWorld.Dispose();
            FrameBuffer frameBuffer = room.FrameBuffer;
            
            // 回滚
            room.LSWorld = room.GetLSWorld(SceneType.LockStepClient, frame);
            OneFrameInputs authorityFrameInput = frameBuffer.FrameInputs(frame);
            // 执行AuthorityFrame
            room.Update(authorityFrameInput);
            // 这里发送Hash去服务器校验, 防止客户端作弊等
            // 下面追帧是一帧内完成的, 没必要重复Send
            room.SendHash(frame);

            
            // 重新执行预测的帧
            // 重新预测的输入都按权威帧输入
            // 这里重新预测和我写的不同, 熊猫没改变权威帧预测帧, 而是int i 去遍历, 防止自己被帧绕晕
            for (int i = room.AuthorityFrame + 1; i <= room.PredictionFrame; ++i)
            {
                OneFrameInputs oneFrameInputs = frameBuffer.FrameInputs(i);
                LSClientHelper.CopyOtherInputsTo(room, authorityFrameInput, oneFrameInputs); // 重新预测消息
                room.Update(oneFrameInputs);
            }
            
            // 调用Room下所有RollbackSystem
            // RollBackSystem是处理表现层, 所以最后调一次就行
            RunLSRollbackSystem(room);
        }
        
        public static void SendHash(this ET.Room self, int frame)
        {
            if (frame > self.AuthorityFrame)
            {
                return;
            }
            long hash = self.FrameBuffer.GetHash(frame);
            C2Room_CheckHash c2RoomCheckHash = C2Room_CheckHash.Create();
            c2RoomCheckHash.Frame = frame;
            c2RoomCheckHash.Hash = hash;
            self.Root().GetComponent<ClientSenderComponent>().Send(c2RoomCheckHash);
        }
        
        // 重新调整预测消息，只需要调整其他玩家的输入
        public static void CopyOtherInputsTo(ET.Room room, OneFrameInputs from, OneFrameInputs to)
        {
            long myId = room.GetComponent<LSClientUpdater>().MyId;
            foreach (var kv in from.Inputs)
            {
                if (kv.Key == myId)
                {
                    continue;
                }
                to.Inputs[kv.Key] = kv.Value;
            }
        }

        public static void SaveReplay(ET.Room room, string path)
        {
            if (room.IsReplay)
            {
                return;
            }
            
            Log.Debug($"save replay: {path} frame: {room.Replay.FrameInputs.Count}");
            byte[] bytes = MemoryPackHelper.Serialize(room.Replay);
            File.WriteAllBytes(path, bytes);
        }
        
        public static void JumpReplay(ET.Room room, int frame)
        {
            if (!room.IsReplay)
            {
                return;
            }

            if (frame >= room.Replay.FrameInputs.Count)
            {
                frame = room.Replay.FrameInputs.Count - 1;
            }
            
            // 要跳转帧处于哪个快照
            int snapshotIndex = frame / LSConstValue.SaveLSWorldFrameCount;
            Log.Debug($"jump replay start {room.AuthorityFrame} {frame} {snapshotIndex}");
            // 跳转帧不在[权威帧, 当前快照结束帧]
            // 跳转只跳转到快照(分钟)上, 具体多少秒自己去运行, 但是会快速跑到那帧
            if (snapshotIndex != room.AuthorityFrame / LSConstValue.SaveLSWorldFrameCount || frame < room.AuthorityFrame)
            {
                room.LSWorld.Dispose();
                // 回滚
                byte[] memoryBuffer = room.Replay.Snapshots[snapshotIndex];
                LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof (LSWorld), memoryBuffer, 0, memoryBuffer.Length) as LSWorld;
                room.LSWorld = lsWorld;
                room.AuthorityFrame = snapshotIndex * LSConstValue.SaveLSWorldFrameCount;
                RunLSRollbackSystem(room);
            }
            
            // 相当于把时间设置到跳转帧的时间
            // LSReplayUpdater就会尽量跑满5ms, 从而快速接近跳转帧
            room.FixedTimeCounter.Reset(TimeInfo.Instance.ServerFrameTime() - frame * LSConstValue.UpdateInterval, 0);

            Log.Debug($"jump replay finish {frame}");
        }
    }
}*/