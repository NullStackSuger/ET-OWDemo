namespace ET.Client
{
    // 如果要改动记得一起改
    
    [Event(SceneType.LSFClientPrediction)]
    [FriendOf(typeof(Cast))]
    public class UnitUseCast_ShowView_Prediction : AEvent<LSWorld, UnitUseCast>
    {
        protected override async ETTask Run(LSWorld world, UnitUseCast a)
        {
            Room room = world.GetParent<Room>();
            
            Cast cast = a.Cast;
            LSUnit castUnit = cast.Unit;
            LSFUnitViewComponent unitViewComponent = room.GetComponent<LSFUnitViewComponent>();
            unitViewComponent.Add(castUnit.Id, "Unit/FireBall.prefab", "FireBall", AnimatorType.FireBall).Coroutine();

            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LSFClientAuthority)]
    [FriendOf(typeof(Cast))]
    public class UnitUseCast_ShowView_Authority : AEvent<LSWorld, UnitUseCast>
    {
        protected override async ETTask Run(LSWorld world, UnitUseCast a)
        {
            await ETTask.CompletedTask;
        }
    }
}