//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/_MyAssets/PlayerInput/PlayerInputActions.inputactions
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

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerSword"",
            ""id"": ""c18d2fad-8311-4bf1-8724-c837542aee79"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""ea15f643-c341-4005-8774-173f196ccf08"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""7618a21b-3185-4b0e-8c0e-d633f6a891d3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""22c620f9-33ce-43a8-94eb-287e988a4846"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RegularAttack"",
                    ""type"": ""Button"",
                    ""id"": ""561259ba-41b5-481d-9af3-4a668fe91b58"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lock-On"",
                    ""type"": ""Button"",
                    ""id"": ""38bed835-41c1-4c48-9177-fa7749165096"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FocusRight"",
                    ""type"": ""Button"",
                    ""id"": ""7b26b3f6-3cab-4a61-82a8-dd3c59150d82"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FocusLeft"",
                    ""type"": ""Button"",
                    ""id"": ""2256820d-c574-4146-9161-65ffc4b20698"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""1692b62b-c3c1-40fb-87e7-3aeaa6e00ce2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""fb1cd734-8407-4800-9bec-863fa36d1326"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""dca825f2-5ec0-4713-93e8-bffdf65b0e9c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6ae5957f-2f3d-4d47-a7f7-7f36ce4a0785"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f9d16fd5-49fa-4553-8722-1a822f183db4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""835ddfd8-b4b9-4530-9ee2-4798dad1bfe2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2d509da0-7a18-4e86-9327-e97b24cdd34e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftStick (Controller)"",
                    ""id"": ""7f5822ab-6beb-44c5-8984-24f1e8847e74"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b1bc9256-8b49-445a-868a-38250b38aff9"",
                    ""path"": ""<DualSenseGamepadHID>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ad795c97-14ea-4ded-9bdd-1dd31745efd4"",
                    ""path"": ""<DualSenseGamepadHID>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""88205d90-217e-40ea-afc3-1f9a380e7053"",
                    ""path"": ""<DualSenseGamepadHID>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bc9cd352-381e-42d7-8fef-24189e93b6f0"",
                    ""path"": ""<DualSenseGamepadHID>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8c7d3cc2-35ce-4789-bc59-c2c7d5f46998"",
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
                    ""id"": ""503cec28-e4f6-43c4-8e39-1750a54882fc"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6df93d84-8513-497a-b243-44d6207f2d2e"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f0845fc-c23e-444c-aa9c-0584581fefd5"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f57fcaa5-6be5-4378-8d21-c69103bc1574"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock-On"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff5acc52-6324-4cd7-9418-4da46991e4fb"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock-On"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a18c0e0d-cd55-4a23-8333-379ecb8c5758"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FocusLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aea460a9-d4e8-49c4-bd6c-552af4103704"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FocusLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a59282e-3dd8-4740-a943-0dff190c6e5f"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FocusRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0104873-3843-43d9-8eab-ddd3fb439bc4"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FocusRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5aebf036-bca5-44ca-b45c-a6285d26a060"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RegularAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5de81dd3-2c6f-4004-81b4-ca9d58577caf"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RegularAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""746a9be2-fcab-4408-ab70-009fc69e2dbb"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad8d835e-5ce4-42ae-b2bb-4d759b14a34b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerSword
        m_PlayerSword = asset.FindActionMap("PlayerSword", throwIfNotFound: true);
        m_PlayerSword_Movement = m_PlayerSword.FindAction("Movement", throwIfNotFound: true);
        m_PlayerSword_Look = m_PlayerSword.FindAction("Look", throwIfNotFound: true);
        m_PlayerSword_Sprint = m_PlayerSword.FindAction("Sprint", throwIfNotFound: true);
        m_PlayerSword_RegularAttack = m_PlayerSword.FindAction("RegularAttack", throwIfNotFound: true);
        m_PlayerSword_LockOn = m_PlayerSword.FindAction("Lock-On", throwIfNotFound: true);
        m_PlayerSword_FocusRight = m_PlayerSword.FindAction("FocusRight", throwIfNotFound: true);
        m_PlayerSword_FocusLeft = m_PlayerSword.FindAction("FocusLeft", throwIfNotFound: true);
        m_PlayerSword_Dodge = m_PlayerSword.FindAction("Dodge", throwIfNotFound: true);
        m_PlayerSword_Menu = m_PlayerSword.FindAction("Menu", throwIfNotFound: true);
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

    // PlayerSword
    private readonly InputActionMap m_PlayerSword;
    private List<IPlayerSwordActions> m_PlayerSwordActionsCallbackInterfaces = new List<IPlayerSwordActions>();
    private readonly InputAction m_PlayerSword_Movement;
    private readonly InputAction m_PlayerSword_Look;
    private readonly InputAction m_PlayerSword_Sprint;
    private readonly InputAction m_PlayerSword_RegularAttack;
    private readonly InputAction m_PlayerSword_LockOn;
    private readonly InputAction m_PlayerSword_FocusRight;
    private readonly InputAction m_PlayerSword_FocusLeft;
    private readonly InputAction m_PlayerSword_Dodge;
    private readonly InputAction m_PlayerSword_Menu;
    public struct PlayerSwordActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerSwordActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerSword_Movement;
        public InputAction @Look => m_Wrapper.m_PlayerSword_Look;
        public InputAction @Sprint => m_Wrapper.m_PlayerSword_Sprint;
        public InputAction @RegularAttack => m_Wrapper.m_PlayerSword_RegularAttack;
        public InputAction @LockOn => m_Wrapper.m_PlayerSword_LockOn;
        public InputAction @FocusRight => m_Wrapper.m_PlayerSword_FocusRight;
        public InputAction @FocusLeft => m_Wrapper.m_PlayerSword_FocusLeft;
        public InputAction @Dodge => m_Wrapper.m_PlayerSword_Dodge;
        public InputAction @Menu => m_Wrapper.m_PlayerSword_Menu;
        public InputActionMap Get() { return m_Wrapper.m_PlayerSword; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerSwordActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerSwordActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerSwordActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerSwordActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @RegularAttack.started += instance.OnRegularAttack;
            @RegularAttack.performed += instance.OnRegularAttack;
            @RegularAttack.canceled += instance.OnRegularAttack;
            @LockOn.started += instance.OnLockOn;
            @LockOn.performed += instance.OnLockOn;
            @LockOn.canceled += instance.OnLockOn;
            @FocusRight.started += instance.OnFocusRight;
            @FocusRight.performed += instance.OnFocusRight;
            @FocusRight.canceled += instance.OnFocusRight;
            @FocusLeft.started += instance.OnFocusLeft;
            @FocusLeft.performed += instance.OnFocusLeft;
            @FocusLeft.canceled += instance.OnFocusLeft;
            @Dodge.started += instance.OnDodge;
            @Dodge.performed += instance.OnDodge;
            @Dodge.canceled += instance.OnDodge;
            @Menu.started += instance.OnMenu;
            @Menu.performed += instance.OnMenu;
            @Menu.canceled += instance.OnMenu;
        }

        private void UnregisterCallbacks(IPlayerSwordActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @RegularAttack.started -= instance.OnRegularAttack;
            @RegularAttack.performed -= instance.OnRegularAttack;
            @RegularAttack.canceled -= instance.OnRegularAttack;
            @LockOn.started -= instance.OnLockOn;
            @LockOn.performed -= instance.OnLockOn;
            @LockOn.canceled -= instance.OnLockOn;
            @FocusRight.started -= instance.OnFocusRight;
            @FocusRight.performed -= instance.OnFocusRight;
            @FocusRight.canceled -= instance.OnFocusRight;
            @FocusLeft.started -= instance.OnFocusLeft;
            @FocusLeft.performed -= instance.OnFocusLeft;
            @FocusLeft.canceled -= instance.OnFocusLeft;
            @Dodge.started -= instance.OnDodge;
            @Dodge.performed -= instance.OnDodge;
            @Dodge.canceled -= instance.OnDodge;
            @Menu.started -= instance.OnMenu;
            @Menu.performed -= instance.OnMenu;
            @Menu.canceled -= instance.OnMenu;
        }

        public void RemoveCallbacks(IPlayerSwordActions instance)
        {
            if (m_Wrapper.m_PlayerSwordActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerSwordActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerSwordActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerSwordActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerSwordActions @PlayerSword => new PlayerSwordActions(this);
    public interface IPlayerSwordActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnRegularAttack(InputAction.CallbackContext context);
        void OnLockOn(InputAction.CallbackContext context);
        void OnFocusRight(InputAction.CallbackContext context);
        void OnFocusLeft(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
    }
}
