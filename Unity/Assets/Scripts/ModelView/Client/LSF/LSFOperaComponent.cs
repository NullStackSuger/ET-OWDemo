using UnityEngine.InputSystem;

namespace ET
{
    [ComponentOf(typeof(Room))]
    public class LSFOperaComponent: Entity, IAwake, IDestroy, IUpdate
    {
        public IInputActionCollection InputSystem;
        
        public InputAction Move;
    }
}