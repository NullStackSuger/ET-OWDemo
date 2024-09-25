namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    public class UnitChangeSnipe_ChangeSnipe : AEvent<LSWorld, UnitChangeSnipe>
    {
        protected override async ETTask Run(LSWorld world, UnitChangeSnipe a)
        {
            Scene scene = world.Root();
            if (UIHelper.Has(scene, UIType.Snipe))
            {
                await UIHelper.Remove(scene, UIType.Snipe);
            }
            else
            {
                await UIHelper.Create(scene, UIType.Snipe, UILayer.Mid);   
            }
        }
    }
}