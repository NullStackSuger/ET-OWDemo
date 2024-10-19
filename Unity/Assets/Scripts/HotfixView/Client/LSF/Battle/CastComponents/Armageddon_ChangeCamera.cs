namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    public class ArmageddonStart_ChangeCamera : AEvent<LSWorld, ArmageddonStart>
    {
        protected override async ETTask Run(LSWorld scene, ArmageddonStart a)
        {
            LSFUnitView view = scene.GetParent<Room>().GetComponent<LSFUnitViewComponent>().GetChild<LSFUnitView>(a.Unit.Id);
            view.RemoveComponent<LSFCameraComponent>();
            view.AddComponent<LSFUpViewCameraComponent>();
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LSFClientPrediction)]
    public class ArmageddonEnd_ChangeCamera : AEvent<LSWorld, ArmageddonEnd>
    {
        protected override async ETTask Run(LSWorld scene, ArmageddonEnd a)
        {
            LSFUnitView view = scene.GetParent<Room>().GetComponent<LSFUnitViewComponent>().GetChild<LSFUnitView>(a.UnitId);
            view.RemoveComponent<LSFUpViewCameraComponent>();
            view.AddComponent<LSFCameraComponent>();
            await ETTask.CompletedTask;
        }
    }
}