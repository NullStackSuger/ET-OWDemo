using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSFAnimatorComponent))]
    [FriendOf(typeof(LSFAnimatorComponent))]
    [FriendOf(typeof(LSFUnitView))]
    public static partial class LSFAnimatorComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSFAnimatorComponent self, string type)
        {
            LSFUnitView unitView = self.GetParent<LSFUnitView>();
            Animator animator = unitView.GameObject.GetComponent<Animator>();
            if (animator == null) return;
            if (animator.runtimeAnimatorController == null) return;
            if (animator.runtimeAnimatorController.animationClips == null) return;
            self.Animator = animator;

            foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
            {
                self.Clips[animationClip.name] = animationClip;
            }
            foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
            {
                self.Parameter.Add(animatorControllerParameter.name);
            }

            self.Type = type;
        }

        [EntitySystem]
        private static void Destroy(this LSFAnimatorComponent self)
        {
            self.Clips = null;
            self.Parameter = null;
            self.Animator = null;
        }

        [EntitySystem]
        private static void Update(this LSFAnimatorComponent self)
        {
            LSUnit unit = self.GetParent<LSFUnitView>().Unit;
            AnimatorDispatcherComponent.Instance[self.Type].Update(self, unit);
        }

        public static bool HasClip(this LSFAnimatorComponent self, string clipName)
        {
            return self.Clips.ContainsKey(clipName);
        }
        public static AnimationClip GetClip(this LSFAnimatorComponent self, string clipName)
        {
            return self.Clips[clipName];
        }

        public static bool TryGetClip(this LSFAnimatorComponent self, string clipName, out AnimationClip clip)
        {
            return self.Clips.TryGetValue(clipName, out clip);
        }
        
        public static bool HasParameter(this LSFAnimatorComponent self, string parameter)
        {
            return self.Parameter.Contains(parameter);
        }

        public static void SetBool(this LSFAnimatorComponent self, string name, bool state)
        {
            if (!self.HasParameter(name))
            {
                return;
            }

            self.Animator.SetBool(name, state);
        }

        public static void SetFloat(this LSFAnimatorComponent self, string name, float state)
        {
            if (!self.HasParameter(name))
            {
                return;
            }

            self.Animator.SetFloat(name, state);
        }

        public static void SetInt(this LSFAnimatorComponent self, string name, int value)
        {
            if (!self.HasParameter(name))
            {
                return;
            }

            self.Animator.SetInteger(name, value);
        }

        public static void SetTrigger(this LSFAnimatorComponent self, string name)
        {
            if (!self.HasParameter(name))
            {
                return;
            }

            self.Animator.SetTrigger(name);
        }
    }
}