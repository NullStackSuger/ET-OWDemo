namespace ET.Client
{
    [EntitySystemOf(typeof(SnipeComponent))]
    public static partial class SnipeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SnipeComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }
        [EntitySystem]
        private static void Update(this SnipeComponent self)
        {

        }
    }
}