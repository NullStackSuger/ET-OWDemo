using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(LSFUnitView))]
    public class LSFAnimatorComponent : Entity, IAwake<AnimatorType>, IDestroy, IUpdate
    {
        public AnimatorType Type;
        
        /// <summary>
        /// 状态机上动画
        /// </summary>
        public Dictionary<string, AnimationClip> Clips = new();
        
        /// <summary>
        /// 状态机参数
        /// </summary>
        public HashSet<string> Parameter = new();

        public Animator Animator;
    }

    public enum AnimatorType
    {
        Skeleton,
        FireBall,
    }
}