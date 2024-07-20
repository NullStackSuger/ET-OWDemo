using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class LSF_UILoginComponent : Entity, IAwake
    {
        public GameObject email;
        public GameObject password;
        public GameObject loginBut;
    }
}