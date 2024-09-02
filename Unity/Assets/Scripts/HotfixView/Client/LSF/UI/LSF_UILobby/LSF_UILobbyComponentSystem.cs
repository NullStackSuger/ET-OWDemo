using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSF_UILobbyComponent))]
    [FriendOf(typeof(LSF_UILobbyComponent))]
    public static partial class LSF_UILobbyComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSF_UILobbyComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.matchBut = rc.Get<GameObject>("MatchBut");
            self.matchBut.GetComponent<Button>().onClick.AddListener(() => { self.EnterMap().Coroutine(); });

            // Player
            for (int i = 0; i < LSFConfig.MatchCount; i++)
            {
                self.players.Add(rc.Get<GameObject>($"Choose {i}").GetComponent<Image>());
            }

            // Role
            for (int i = 0; i < 5 + 1; i++)
            {
                GameObject obj = rc.Get<GameObject>($"Role {i}");
                Button but = obj.GetComponent<Button>();
                self.roles.Add($"Role {i}", obj);
                but.onClick.AddListener(self.OnRoleClick);
            }
        }

        private static async ETTask EnterMap(this LSF_UILobbyComponent self)
        {
            string path = "D:\\RP.replay";
            if (File.Exists(path))
            {
                byte[] bytes = await File.ReadAllBytesAsync(path);
                Replay replay = MemoryPackHelper.Deserialize(typeof(Replay), bytes, 0, bytes.Length) as Replay;
                LSSceneChangeHelper.SceneChangeToReplay(self.Root(), replay).Coroutine();
                await ETTask.CompletedTask;
            }
            else
            {
                await EnterMapHelper.Match(self.Fiber());
            }
        }

        private static void OnRoleClick(this LSF_UILobbyComponent self)
        {
            
        }
    }
}