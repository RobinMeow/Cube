//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/UserInputs/DeviceInputs.inputactions
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

public partial class @DeviceInputs : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @DeviceInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DeviceInputs"",
    ""maps"": [
        {
            ""name"": ""CubeShooter"",
            ""id"": ""85878a20-3cb0-40e9-a335-64ebb933f445"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""8d55217c-3df8-471b-8c9d-48d8c6d7e55f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""c3442ac9-35a0-4d0d-b14e-525864991268"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4543d92c-86fa-4b9a-b15e-3a29eba08caf"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db783621-a51f-436f-9577-67282a5ec881"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""AD"",
                    ""id"": ""d45fcb0e-3164-43f5-b12d-70226ee91d45"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ff6bc4b3-8108-42f0-aa97-caba1d549e51"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""fba7134b-e119-426c-afe3-e946acc113df"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftStick"",
                    ""id"": ""66c13d5c-492d-46d5-a8cd-9033327e176a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""6ccf2a67-2354-4c41-bc9d-f869e0ff93c8"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""51100194-8973-474e-b66c-afa03e2740af"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CubeShooter
        m_CubeShooter = asset.FindActionMap("CubeShooter", throwIfNotFound: true);
        m_CubeShooter_Jump = m_CubeShooter.FindAction("Jump", throwIfNotFound: true);
        m_CubeShooter_Move = m_CubeShooter.FindAction("Move", throwIfNotFound: true);
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

    // CubeShooter
    private readonly InputActionMap m_CubeShooter;
    private ICubeShooterActions m_CubeShooterActionsCallbackInterface;
    private readonly InputAction m_CubeShooter_Jump;
    private readonly InputAction m_CubeShooter_Move;
    public struct CubeShooterActions
    {
        private @DeviceInputs m_Wrapper;
        public CubeShooterActions(@DeviceInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_CubeShooter_Jump;
        public InputAction @Move => m_Wrapper.m_CubeShooter_Move;
        public InputActionMap Get() { return m_Wrapper.m_CubeShooter; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CubeShooterActions set) { return set.Get(); }
        public void SetCallbacks(ICubeShooterActions instance)
        {
            if (m_Wrapper.m_CubeShooterActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_CubeShooterActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_CubeShooterActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_CubeShooterActionsCallbackInterface.OnJump;
                @Move.started -= m_Wrapper.m_CubeShooterActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CubeShooterActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CubeShooterActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_CubeShooterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public CubeShooterActions @CubeShooter => new CubeShooterActions(this);
    public interface ICubeShooterActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
    }
}