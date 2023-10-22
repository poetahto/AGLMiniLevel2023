using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AGL.Player
{
    public class PlayerRotationTest : MonoBehaviour
    {
        [SerializeField] private PlayerCameraController m_camera;

        private Quaternion target = Quaternion.identity;

        private void Update()
        {
            if (Keyboard.current.wKey.isPressed)
            {
                target = Quaternion.LookRotation(m_camera.LookDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 100f * Time.deltaTime);
            }
        }
    }
}
