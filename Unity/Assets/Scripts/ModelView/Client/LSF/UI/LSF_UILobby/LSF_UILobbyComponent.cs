using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class LSF_UILobbyComponent : Entity, IAwake
    {
        public List<Image> players = new(LSFConfig.MatchCount);
        public Dictionary<string, GameObject> roles = new();
        public GameObject matchBut;
    }
}