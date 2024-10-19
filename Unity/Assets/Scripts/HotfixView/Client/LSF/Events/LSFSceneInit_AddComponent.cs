using System.Linq;
using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepFrame)]
    public class LSFSceneInit_AddComponent : AEvent<Scene, LSFSceneInit>
    {
        protected override async ETTask Run(Scene scene, LSFSceneInit a)
        {
            Room room = scene.GetComponent<Room>();

            await room.AddComponent<LSFUnitViewComponent>().InitPlayerAsync("Unit/Unit.prefab", "Skeleton");

            room.GetComponent<LSFUnitViewComponent>().GetChild<LSFUnitView>(room.PlayerId).AddComponent<LSFCameraComponent>();

            bool isRobot = scene.Name.StartsWith("Robot");
            
            if (!room.IsReplay && !isRobot)
            {
                room.AddComponent<LSFOperaComponent>();
            }

            if (isRobot)
            {
                AIComponent aiComponent = scene.AddComponent<AIComponent, int>(3);
                aiComponent.AddComponent<RobotInputFinishComponent>();
            }
        }
    }
}