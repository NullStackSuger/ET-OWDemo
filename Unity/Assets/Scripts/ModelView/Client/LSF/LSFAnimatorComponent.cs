using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(LSFUnitView))]
    public class LSFAnimatorComponent : Entity, IAwake<string>, IDestroy, IUpdate
    {
        public string Type;
        
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

    public static class AnimatorType
    {
        public const string Skeleton = "Skeleton";
        public const string FireBall = "FireBall";
        public const string HpStick = "HpStick";
    }
}