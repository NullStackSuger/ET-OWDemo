namespace ET.Client
{
    [Event(SceneType.LSFClientAuthority)]
    [FriendOfAttribute(typeof(ET.Cast))]
    public class UnitUseCast_AddComponent1 : AEvent<LSWorld, UnitUseCast>
    {
        protected override async ETTask Run(LSWorld scene, UnitUseCast a)
        {
            LSUnit castUnit = a.Cast.Unit;
            castUnit.AddComponent<DataModifierComponent>();

            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LSFClientPrediction)]
    [FriendOfAttribute(typeof(ET.Cast))]
    public class UnitUseCast_AddComponent2 : AEvent<LSWorld, UnitUseCast>
    {
        protected override async ETTask Run(LSWorld scene, UnitUseCast a)
        {
            LSUnit castUnit = a.Cast.Unit;
            castUnit.AddComponent<DataModifierComponent>();

            await ETTask.CompletedTask;
        }
    }
}