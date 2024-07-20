/*using System;
using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(LSServerUpdater))]
    [FriendOf(typeof(LSServerUpdater))]
    public static partial class LSServerUpdaterSystem
    {
        [EntitySystem]
        private static void Awake(this LSServerUpdater self)
        {

        }
        
        [EntitySystem]
        private static void Update(this LSServerUpdater self)
        {
            ET.Room room = self.GetParent<ET.Room>();
            long timeNow = TimeInfo.Instance.ServerFrameTime();


            int frame = room.AuthorityFrame + 1;
            if (timeNow < room.FixedTimeCounter.FrameTime(frame))
            {
                return;
            }

            OneFrameInputs oneFrameInputs = self.GetOneFrameMessage(frame);
            ++room.AuthorityFrame;
            
            Log.Warning($"Server Frame {room.AuthorityFrame}");

            // 因为是多线程, 不能直接广播oneFrameInputs, 之后可能会改它
            OneFrameInputs sendInput = OneFrameInputs.Create();
            oneFrameInputs.CopyTo(sendInput);
            RoomMessageHelper.BroadCast(room, sendInput);

            room.Update(oneFrameInputs);
        }

        private static OneFrameInputs GetOneFrameMessage(this LSServerUpdater self, int frame)
        {
            ET.Room room = self.GetParent<ET.Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            OneFrameInputs oneFrameInputs = frameBuffer.FrameInputs(frame);
            frameBuffer.MoveForward(frame);

            // 输入数量 == 房间人数 说明一切正常
            if (oneFrameInputs.Inputs.Count == LSConstValue.MatchCount)
            {
                return oneFrameInputs;
            }

            // 输入数量 != 房间人数, 就要用上帧输入
            OneFrameInputs preFrameInputs = null;
            if (frameBuffer.CheckFrame(frame - 1))
            {
                preFrameInputs = frameBuffer.FrameInputs(frame - 1);
            }

            // 有人输入的消息没过来，给他使用上一帧的操作
            foreach (long playerId in room.PlayerIds)
            {
                if (oneFrameInputs.Inputs.ContainsKey(playerId))
                {
                    continue;
                }

                if (preFrameInputs != null && preFrameInputs.Inputs.TryGetValue(playerId, out LSInput input))
                {
                    // 使用上一帧的输入
                    oneFrameInputs.Inputs[playerId] = input;
                }
                else // 第0帧没前一帧 就new一下
                {
                    oneFrameInputs.Inputs[playerId] = new LSInput();
                }
            }

            return oneFrameInputs;
        }
    }
}*/