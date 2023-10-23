using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private InputHandler m_actionAsset;
    [SerializeField] private Inventory m_inventory;
    public Inventory PlayerInventory => m_inventory;

    private UIDocument m_ui;

    private void Awake()
    {
        m_ui = GetComponentInChildren<UIDocument>();
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            m_ui.rootVisualElement.style.display = m_ui.rootVisualElement.style.display != DisplayStyle.None ? DisplayStyle.None : DisplayStyle.Flex;
            m_actionAsset.IsPaused = !m_actionAsset.IsPaused;
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}

