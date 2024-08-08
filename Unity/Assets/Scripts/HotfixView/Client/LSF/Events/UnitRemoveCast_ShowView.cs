namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    [FriendOf(typeof(Cast))]
    public class UnitRemoveCast_ShowView_Prediction : AEvent<LSWorld, UnitRemoveCast>
    {
        protected override async ETTask Run(LSWorld world, UnitRemoveCast a)
        {
            Log.Warning($"Prediction 移除UnitView");
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LSFClientAuthority)]
    [FriendOf(typeof(Cast))]
    public class UnitRemoveCast_ShowView_Authority : AEvent<LSWorld, UnitRemoveCast>
    {
        protected override async ETTask Run(LSWorld world, UnitRemoveCast a)
        {
            Log.Warning($"Authority 移除UnitView");
            await ETTask.CompletedTask;
        }
    }
}