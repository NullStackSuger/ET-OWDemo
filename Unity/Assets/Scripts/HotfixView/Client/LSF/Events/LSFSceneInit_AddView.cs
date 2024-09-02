using System.Linq;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class LSFSceneInit_AddView : AEvent<Scene, LSFSceneInit>
    {
        protected override async ETTask Run(Scene scene, LSFSceneInit a)
        {
            Room room = scene.GetComponent<Room>();

            await room.AddComponent<LSFUnitViewComponent>().InitPlayerAsync("Unit/Unit.prefab", "Skeleton", AnimatorType.Skeleton);

            room.AddComponent<LSFCameraComponent>();

            if (!room.IsReplay)
            {
                // TODO 这里要根据是真人还是玩家, 添加不同的OperaComponent
                room.AddComponent<LSFOperaComponent>();
            }
        }
    }
}