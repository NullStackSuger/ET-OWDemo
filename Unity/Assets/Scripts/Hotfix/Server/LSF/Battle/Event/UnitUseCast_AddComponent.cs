namespace ET.Server
{
    [Event(SceneType.LSFServer)]
    [FriendOfAttribute(typeof(ET.Cast))]
    public class UnitUseCast_AddComponent : AEvent<LSWorld, UnitUseCast>
    {
        protected override async ETTask Run(LSWorld world, UnitUseCast a)
        {
            LSUnit castUnit = a.Cast.Unit;
            castUnit.AddComponent<DataModifierComponent>();
            
            CastConfig config = CastConfigCategory.Instance.Get(a.Cast.ConfigId);
            
            // 添加碰撞
            if (config.RigidBody != 0)
            {
                castUnit.AddComponent<B3CollisionComponent, int>(config.RigidBody);
            }

            await ETTask.CompletedTask;
        }
    }
}