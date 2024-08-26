namespace ET
{
    public enum CollisionMaskType
    {
        True, 
        False,
        /// <summary>
        /// 和自己不相同的
        /// </summary>
        SelfDiff,
        /// <summary>
        /// 和自己相同的
        /// </summary>
        Self,
    }
}