using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using MiniTools.BetterGizmos;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private InputHandler m_actionAsset;
    [SerializeField] private Inventory m_inventory;
    public Inventory PlayerInventory => m_inventory;

    private InventoryUIController m_ui;

    private void Awake()
    {
        m_ui = GetComponentInChildren<InventoryUIController>();
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            m_ui.Display(!m_ui.IsDisplaying);
            m_actionAsset.IsPaused = !m_actionAsset.IsPaused;
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
        }

        var collectables = Physics.OverlapSphere(transform.position, 5f).Where(c => c.TryGetComponent(out ICollectable o)).ToList();

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (collectables.Count == 0) return;
            
            if (collectables[0].TryGetComponent(out ICollectable c))
            {
                c.Collect(m_inventory);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}

