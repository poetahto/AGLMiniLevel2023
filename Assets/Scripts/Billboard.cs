using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera m_camera;

    private void Awake()
    {
        m_camera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(m_camera.transform, Vector3.up);
    }
}
