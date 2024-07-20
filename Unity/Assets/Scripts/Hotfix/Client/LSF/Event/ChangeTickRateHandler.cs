using System;

namespace ET.Client
{
    [MessageHandler(SceneType.LockStepFrame)]
    public class ChangeTickRateHandler : MessageHandler<Scene, Room2C_AdjustUpdateTime>
    {
        protected override async ETTask Run(Scene entity, Room2C_AdjustUpdateTime message)
        {
            Room room = entity.GetComponent<Room>();
            int newInterval = (1000 + (message.DiffTime - LSFConfig.NormalTickRate)) * LSFConfig.NormalTickRate / 1000;
            
            if (newInterval < LSFConfig.MinTickRate)
            {
                newInterval = LSFConfig.MinTickRate;
            }

            if (newInterval > LSFConfig.MaxTickRate)
            {
                newInterval = LSFConfig.MaxTickRate;
            }

            room.FixedTimeCounter.ChangeInterval(newInterval, room.PredictionFrame);
            
            await ETTask.CompletedTask;
        }   
    }
}