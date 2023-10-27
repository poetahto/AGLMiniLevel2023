using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


    [CreateAssetMenu(fileName = "InputReaderAsset", menuName = "System/InputReader")]
    public class InputHandler : ScriptableObject, Controls.IDefaultActions
    {
        private Controls m_gameInput;

        public event Action<Vector2> OnMoveEvent;
        public event Action<float> OnLookYawEvent;
        public event Action<float> OnLookPitchEvent;
        public event Action OnJumpEvent;
        public event Action OnDiveEvent;
        public event Action OnInventoryEvent;
        public event Action OnInteractEvent;

        public float LookYaw => IsPaused ? 0f : m_gameInput.Default.LookYaw.ReadValue<float>();
        public float LookPitch => IsPaused ? 0f : m_gameInput.Default.LookPitch.ReadValue<float>();

        public bool IsDisabled { get; private set; } = false;
        public bool IsPaused { get; set; } = false;

        private void OnEnable()
        {
            IsDisabled = false;
            IsPaused = false;

            if (m_gameInput == null)
            {
                m_gameInput = new Controls();
                m_gameInput.Default.SetCallbacks(this);
            }

            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        public void SetActive(bool _value)
        {
            IsDisabled = !_value;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(IsPaused) return;

            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnDive(InputAction.CallbackContext context)
        {
            if (IsPaused) return;

            if (context.action.WasPressedThisFrame())
            {
                OnDiveEvent?.Invoke();
            }
        }

        public void OnLookYaw(InputAction.CallbackContext context)
        {
            if (IsPaused) return;

            OnLookYawEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnLookPitch(InputAction.CallbackContext context)
        {
            if (IsPaused) return;

            OnLookPitchEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (IsPaused) return;

            OnInteractEvent?.Invoke();
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            OnInventoryEvent?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(IsPaused) return;

            if (context.action.WasPressedThisFrame())
            {
                OnJumpEvent?.Invoke();
            }
        }

        private void EnableInput()
        {
            m_gameInput.Default.Enable();
        }

        private void DisableInput()
        {
            m_gameInput.Default.Disable();
        }
    }

