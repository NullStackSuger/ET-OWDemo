//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Res/Unit/Skeleton/SkeletonInputSystem.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @SkeletonInputSystem: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @SkeletonInputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""SkeletonInputSystem"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""95f97252-ca1d-42cf-95f5-0c5ed0dccb5f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""93ced89d-8452-4fd1-8e2d-045a5af0465d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""b28c41d9-509f-423f-a991-a66375ca21aa"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""4eb7c943-50d9-423b-a485-187c444547c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cast_Q"",
                    ""type"": ""Button"",
                    ""id"": ""ea8c285f-cccd-4382-9643-c3a8f20b29eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cast_E"",
                    ""type"": ""Button"",
                    ""id"": ""920a142c-6789-4ab7-8ce3-44fd74d7db09"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cast_C"",
                    ""type"": ""Button"",
                    ""id"": ""7bd1e80b-2211-4b86-ac70-298f5bfac4f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeBulletCount"",
                    ""type"": ""Button"",
                    ""id"": ""fc6eb8a7-e7cc-4949-a18c-b99681cb5f86"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interactive"",
                    ""type"": ""Button"",
                    ""id"": ""5c4c1293-01d6-4846-bc68-5677b44fce04"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""852ede59-df37-49e3-9bd4-7ca06a3cbadf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7b3d117a-4a08-4adb-bc84-2150f8d9995a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""eceef74a-3797-4e35-bba2-d7fe995d51c2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b08ab2ec-481b-44e6-9155-9725d0cc877c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""229ae5be-aa1f-4b4a-a38c-b05779b10c19"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""33121ad6-9948-40d2-be7b-261b06922657"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea9a3f93-2def-42fd-9643-a3101ae4f125"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""526d7776-5182-44a1-a08f-9940e3ee47b1"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cast_Q"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13609a6d-a67f-4258-b67f-f32e6576e4e5"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cast_C"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e7a2793-6658-4ac1-8f2a-72c901869d71"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interactive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f96d2563-2456-4549-ac61-a6eb93d20a19"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cast_E"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9f1368a-be2e-4e5c-80f8-48ab8b766aaa"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeBulletCount"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_Move = m_GamePlay.FindAction("Move", throwIfNotFound: true);
        m_GamePlay_Look = m_GamePlay.FindAction("Look", throwIfNotFound: true);
        m_GamePlay_Jump = m_GamePlay.FindAction("Jump", throwIfNotFound: true);
        m_GamePlay_Cast_Q = m_GamePlay.FindAction("Cast_Q", throwIfNotFound: true);
        m_GamePlay_Cast_E = m_GamePlay.FindAction("Cast_E", throwIfNotFound: true);
        m_GamePlay_Cast_C = m_GamePlay.FindAction("Cast_C", throwIfNotFound: true);
        m_GamePlay_ChangeBulletCount = m_GamePlay.FindAction("ChangeBulletCount", throwIfNotFound: true);
        m_GamePlay_Interactive = m_GamePlay.FindAction("Interactive", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private List<IGamePlayActions> m_GamePlayActionsCallbackInterfaces = new List<IGamePlayActions>();
    private readonly InputAction m_GamePlay_Move;
    private readonly InputAction m_GamePlay_Look;
    private readonly InputAction m_GamePlay_Jump;
    private readonly InputAction m_GamePlay_Cast_Q;
    private readonly InputAction m_GamePlay_Cast_E;
    private readonly InputAction m_GamePlay_Cast_C;
    private readonly InputAction m_GamePlay_ChangeBulletCount;
    private readonly InputAction m_GamePlay_Interactive;
    public struct GamePlayActions
    {
        private @SkeletonInputSystem m_Wrapper;
        public GamePlayActions(@SkeletonInputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_GamePlay_Move;
        public InputAction @Look => m_Wrapper.m_GamePlay_Look;
        public InputAction @Jump => m_Wrapper.m_GamePlay_Jump;
        public InputAction @Cast_Q => m_Wrapper.m_GamePlay_Cast_Q;
        public InputAction @Cast_E => m_Wrapper.m_GamePlay_Cast_E;
        public InputAction @Cast_C => m_Wrapper.m_GamePlay_Cast_C;
        public InputAction @ChangeBulletCount => m_Wrapper.m_GamePlay_ChangeBulletCount;
        public InputAction @Interactive => m_Wrapper.m_GamePlay_Interactive;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void AddCallbacks(IGamePlayActions instance)
        {
            if (instance == null || m_Wrapper.m_GamePlayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GamePlayActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Cast_Q.started += instance.OnCast_Q;
            @Cast_Q.performed += instance.OnCast_Q;
            @Cast_Q.canceled += instance.OnCast_Q;
            @Cast_E.started += instance.OnCast_E;
            @Cast_E.performed += instance.OnCast_E;
            @Cast_E.canceled += instance.OnCast_E;
            @Cast_C.started += instance.OnCast_C;
            @Cast_C.performed += instance.OnCast_C;
            @Cast_C.canceled += instance.OnCast_C;
            @ChangeBulletCount.started += instance.OnChangeBulletCount;
            @ChangeBulletCount.performed += instance.OnChangeBulletCount;
            @ChangeBulletCount.canceled += instance.OnChangeBulletCount;
            @Interactive.started += instance.OnInteractive;
            @Interactive.performed += instance.OnInteractive;
            @Interactive.canceled += instance.OnInteractive;
        }

        private void UnregisterCallbacks(IGamePlayActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Cast_Q.started -= instance.OnCast_Q;
            @Cast_Q.performed -= instance.OnCast_Q;
            @Cast_Q.canceled -= instance.OnCast_Q;
            @Cast_E.started -= instance.OnCast_E;
            @Cast_E.performed -= instance.OnCast_E;
            @Cast_E.canceled -= instance.OnCast_E;
            @Cast_C.started -= instance.OnCast_C;
            @Cast_C.performed -= instance.OnCast_C;
            @Cast_C.canceled -= instance.OnCast_C;
            @ChangeBulletCount.started -= instance.OnChangeBulletCount;
            @ChangeBulletCount.performed -= instance.OnChangeBulletCount;
            @ChangeBulletCount.canceled -= instance.OnChangeBulletCount;
            @Interactive.started -= instance.OnInteractive;
            @Interactive.performed -= instance.OnInteractive;
            @Interactive.canceled -= instance.OnInteractive;
        }

        public void RemoveCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGamePlayActions instance)
        {
            foreach (var item in m_Wrapper.m_GamePlayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GamePlayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);
    public interface IGamePlayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCast_Q(InputAction.CallbackContext context);
        void OnCast_E(InputAction.CallbackContext context);
        void OnCast_C(InputAction.CallbackContext context);
        void OnChangeBulletCount(InputAction.CallbackContext context);
        void OnInteractive(InputAction.CallbackContext context);
    }
}
