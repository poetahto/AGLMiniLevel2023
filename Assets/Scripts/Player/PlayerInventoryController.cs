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


[RequireComponent(typeof(Selector))]
public class PlayerInventoryController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputHandler m_actionAsset;
    [SerializeField] private Inventory m_inventory;
    
    public Inventory PlayerInventory => m_inventory;

    private InventoryUIController m_ui;
    private Selector m_selector;

    private void Awake()
    {
        m_ui = GetComponentInChildren<InventoryUIController>();
        m_selector = GetComponent<Selector>();
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
        

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            var collectable = m_selector.GetCollectables().FirstOrDefault();
            if (collectable != null)
            {
                StartCoroutine(Collect(m_selector.GetCollectableTransform(collectable), collectable, 1f));
            }

            var interactable = m_selector.GetIntractable().FirstOrDefault();
            interactable?.Interact(transform);
        }
    }

    private IEnumerator Collect(Transform t, ICollectable c, float time)
    {
        var scale = t.localScale;
        var position = t.position;

        float tt = 0;
        while (tt < time)
        {
            yield return null;
            t.localScale = Vector3.Lerp(scale, Vector3.zero, tt);
            t.position = Vector3.Lerp(position, transform.position, tt);
            tt += Time.deltaTime;
        }
        
        c.Collect(m_inventory);

        yield return null;
    }
}

