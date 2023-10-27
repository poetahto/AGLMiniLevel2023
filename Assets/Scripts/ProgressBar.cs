using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Image m_fill;

    public float Fill
    {
        set => m_fill.fillAmount = value;
        get => m_fill.fillAmount;
    }
    
    private void Awake()
    {
        m_fill = GetComponentInChildren<Image>();
    }
}
