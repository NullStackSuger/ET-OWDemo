using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LSFClientPrediction)]
    public class UnitChangeMoveSpeed_PlayAnimator : AEvent<LSWorld, UnitChangeMoveSpeed>
    {
        protected override async ETTask Run(LSWorld scene, UnitChangeMoveSpeed a)
        {
            LSFUnitView view = scene.GetParent<Room>().GetComponent<LSFUnitViewComponent>().GetChild<LSFUnitView>(a.Unit.Id);

            // 有可能第1帧Server的Update, View还没创建
            if (view == null)
            {
                return;
            }
            
            var animator = view.GetComponent<AnimancerComponent>();
            if (a.Speed == 0)
            {
                animator.Play(AnimatorState.Idle);
            }
            else
            {
                animator.Play(AnimatorState.Move);
                //animator.Play(AnimatorState.Move, new Vector2(1, 0));
                //animator.Play(AnimatorState.Move, 1);
            }
            
            await ETTask.CompletedTask;
        }
    }
}