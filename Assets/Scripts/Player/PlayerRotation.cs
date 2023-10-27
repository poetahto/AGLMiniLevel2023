using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AGL.Player
{
    public class PlayerRotationTest : MonoBehaviour
    {
        [SerializeField] private float m_speed;
        [SerializeField] private PlayerCameraController m_camera;

        private Quaternion target = Quaternion.identity;

        private void Update()
        {
            if (Keyboard.current.wKey.isPressed)
            {
                transform.Translate(transform.forward * (m_speed * Time.deltaTime));
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                transform.Translate(-transform.forward * (m_speed * Time.deltaTime));
            }
            else if (Keyboard.current.aKey.isPressed)
            {
                transform.Translate(-transform.right * (m_speed * Time.deltaTime));
            }
            else if (Keyboard.current.dKey.isPressed)
            {
                transform.Translate(transform.right * (m_speed * Time.deltaTime));
            }
        }

        private void Rotate(Vector3 dir)
        {
            target = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 100f * Time.deltaTime);
        }
    }
}
