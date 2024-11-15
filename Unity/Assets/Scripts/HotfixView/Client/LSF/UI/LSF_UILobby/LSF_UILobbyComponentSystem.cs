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
            self.matchBut.GetComponent<Button>().onClick.AddListener(() => { self.Match().Coroutine(); });
            self.replayBut = rc.Get<GameObject>("ReplayBut");
            self.replayBut.GetComponent<Button>().onClick.AddListener(() => { self.Replay().Coroutine(); });
        }

        private static async ETTask Match(this LSF_UILobbyComponent self)
        {
            await EnterMapHelper.Match(self.Fiber());
        }

        private static async ETTask Replay(this LSF_UILobbyComponent self)
        {
            string path = "D:\\Replay.rp";
            if (!File.Exists(path)) return;
            
            byte[] bytes = await File.ReadAllBytesAsync(path);
            Replay replay = MemoryPackHelper.Deserialize(typeof(Replay), bytes, 0, bytes.Length) as Replay;
            LSSceneChangeHelper.SceneChangeToReplay(self.Root(), replay).Coroutine();
        }
    }
}