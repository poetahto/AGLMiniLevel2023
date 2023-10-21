using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace AGL.Player
{ 
    /// <summary>
    /// Provides controls for the third person camera. This MonoBehaviour
    /// implements the <c>AxisState.IInputAxisProvider</c> interface which allows it
    /// to override the inputs for the <c>Cinemachine</c> cameras. This script
    /// also performs dampening on the input values to get a smoother rotation.
    /// </summary>
    public class PlayerCameraController : MonoBehaviour, AxisState.IInputAxisProvider
    {
        [Serializable]
        internal struct Sensitivity
        {
            [SerializeField, Range(0.0f, 100.0f)] private float x;
            [SerializeField, Range(0.0f, 100.0f)] private float y;

            public float xSensitivity => x / 100.0f;
            public float ySensitivity => y / 100.0f;
        }

        [SerializeField] private Camera m_camera;
        [Tooltip("The reference to the Input Action that will control the camera rotation")]
        [SerializeField] private InputActionReference m_lookInputAction;
        
        [Header("Preferences")]
        [Tooltip("The time it will take for the smoothing function to reach the target value")]
        [SerializeField] private float m_rotationSmoothTime;
        [Tooltip("The mouse sensitivity divided by axis")]
        [SerializeField] private Sensitivity m_sensitivity;

        /// <summary>
        /// Returns the normalized forward direction of the camera without regard for its local rotation
        /// </summary>
        public Vector3 LookDirection => (targetLookPosition - transform.position).normalized;

        private Vector3 targetLookPosition => new(m_target.position.x, transform.position.y, m_target.position.z);
            
        
        private CinemachineVirtualCameraBase m_cinemachine;
        private Transform m_target;
        private InputAction m_action;
        
        private float m_smoothPitch;
        private float m_pitchSmoothVelocity;
        
        private float m_smoothYaw;
        private float m_yawSmoothVelocity;

        private void OnValidate()
        {
            if (m_lookInputAction == null) return;
            
            if (!m_lookInputAction.action.expectedControlType.Contains("Vector2"))
            {
                Debug.LogError($"The InputAction must have a controlType of 'Vector2'");
            }
        }

        private void Awake()
        {
            m_action = m_lookInputAction.action;
            m_action.Enable();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            m_cinemachine = GetComponent<CinemachineVirtualCameraBase>();
            m_target = m_cinemachine.LookAt;
        }

        private void Update()
        {
            Debug.DrawLine(transform.position, targetLookPosition, Color.green);
        }

        public float GetAxisValue(int axis)
        {
            if (!enabled) return 0.0f;

            var value = m_action.ReadValue<Vector2>().normalized;
            
            return axis switch
            {
                0 => GetYaw(value.x),
                1 => GetPitch(value.y),
                _ => 0.0f
            };
        }

        /// <summary>
        /// Smooths the pitch values
        /// </summary>
        /// <param name="y">axis input from the mouse</param>
        /// <returns>The target pitch value</returns>
        private float GetPitch(float y)
        {
            var target = y * m_sensitivity.ySensitivity;
            
            m_smoothPitch = Mathf.SmoothDampAngle(
                m_smoothPitch,
                target,
                ref m_pitchSmoothVelocity,
                m_rotationSmoothTime);

            return m_smoothPitch;
        }

        /// <summary>
        /// Smooths the yaw values
        /// </summary>
        /// <param name="x">axis input from the mouse</param>
        /// <returns>The target yaw value</returns>
        private float GetYaw(float x)
        {
            var target = x * m_sensitivity.xSensitivity;
            
            m_smoothYaw = Mathf.SmoothDampAngle(
                m_smoothYaw,
                target,
                ref m_yawSmoothVelocity,
                m_rotationSmoothTime);

            return m_smoothYaw;
        }

        private void OnDrawGizmos()
        {
            if (m_target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(m_target.position, m_target.up * 10f);
            }
        }
    }
}
