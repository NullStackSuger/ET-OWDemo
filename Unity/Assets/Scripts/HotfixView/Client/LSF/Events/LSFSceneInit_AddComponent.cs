using System.Linq;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class LSFSceneInit_AddComponent : AEvent<Scene, LSFSceneInit>
    {
        protected override async ETTask Run(Scene scene, LSFSceneInit a)
        {
            Room room = scene.GetComponent<Room>();

            await room.PredictionWorld.AddComponent<LSFUnitViewComponent>().InitPlayerAsync("Unit/Unit.prefab", "Skeleton", AnimatorType.Skeleton);

            room.AddComponent<LSFCameraComponent>();

            if (!room.IsReplay)
            {
                room.AddComponent<LSFOperaComponent>();
                await room.AuthorityWorld.AddComponent<LSFUnitViewComponent>().InitPlayerAsync("Unit/Unit.prefab", "Skeleton");
            }
        }
    }
}