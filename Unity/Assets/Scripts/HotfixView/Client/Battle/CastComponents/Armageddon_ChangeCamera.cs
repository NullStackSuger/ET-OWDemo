namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    public class ArmageddonStart_ChangeCamera : AEvent<LSWorld, ArmageddonStart>
    {
        protected override async ETTask Run(LSWorld scene, ArmageddonStart a)
        {
            Room room = scene.GetParent<Room>();
            room.RemoveComponent<LSFCameraComponent>();
            room.AddComponent<LSFUpViewCameraComponent, LSUnit>(a.Unit);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LSFClientAuthority)]
    public class ArmageddonEnd_ChangeCamera : AEvent<LSWorld, ArmageddonEnd>
    {
        protected override async ETTask Run(LSWorld scene, ArmageddonEnd a)
        {
            Room room = scene.GetParent<Room>();
            room.RemoveComponent<LSFUpViewCameraComponent>();
            room.AddComponent<LSFCameraComponent, long>(a.UnitId);
            await ETTask.CompletedTask;
        }
    }
}