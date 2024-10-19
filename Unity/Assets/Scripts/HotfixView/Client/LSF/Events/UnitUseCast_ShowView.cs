namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    [FriendOf(typeof(Cast))]
    public class UnitUseCast_ShowView : AEvent<LSWorld, UnitUseCast>
    {
        protected override async ETTask Run(LSWorld world, UnitUseCast a)
        {
            Room room = world.GetParent<Room>();
            
            Cast cast = a.Cast;
            LSUnit castUnit = cast.Unit;
            LSFUnitViewComponent unitViewComponent = room.GetComponent<LSFUnitViewComponent>();
            unitViewComponent.Add(castUnit.Id, $"Unit/{a.Name}.prefab", a.Name).Coroutine();

            await ETTask.CompletedTask;
        }
    }
}