using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace AGL.Player
{
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
        
        [SerializeField] private InputActionReference m_lookInputAction;
        [Space] 
        [SerializeField] private Sensitivity m_sensitivity;

        public float GetAxisValue(int axis)
        {
            if (!enabled) return 0.0f;
            if (m_lookInputAction == null) return 0.0f;

            m_lookInputAction.action.Enable();
            
            return axis switch
            {
                0 => m_lookInputAction.action.ReadValue<Vector2>().x * m_sensitivity.xSensitivity,
                1 => m_lookInputAction.action.ReadValue<Vector2>().y * m_sensitivity.ySensitivity,
                _ => 0.0f
            };
        }
    }
}
