using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{

    [RequireComponent(typeof(MeshFilter))]
    public class ET_MeshCollider : MonoBehaviour
    {
        [ShowInInspector]
        public float Mass { get; private set; } = 0;
    }
}