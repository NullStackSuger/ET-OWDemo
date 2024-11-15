namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    public class UnitChangePosition_UpdateAOI : AEvent<LSWorld, UnitChangePosition>
    {
        protected override async ETTask Run(LSWorld scene, UnitChangePosition a)
        {
            LSUnit unit = a.Unit;
            AOIEntity aoi = unit.GetComponent<AOIEntity>();
            if (aoi == null) return;
            aoi.Move((float)unit.Position.x / AOIManagerComponent.CellSize, (float)unit.Position.z / AOIManagerComponent.CellSize);
            await ETTask.CompletedTask;
        }
    }
}