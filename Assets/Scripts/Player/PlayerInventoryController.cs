using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AGL.Player;
using InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;


[RequireComponent(typeof(Selector))]
public class PlayerInventoryController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputHandler m_actionAsset;
    [SerializeField] private Inventory m_inventory;
    [SerializeField] private AudioSource m_collectSound;
    [SerializeField] private AudioSource m_collectStartSound;
    [SerializeField] private PlayerAnimator m_playerAnimator;

    public Inventory PlayerInventory => m_inventory;

    private InventoryUIController m_ui;
    private Selector m_selector;
    private HashSet<ICollectable> m_animatingCollectables;

    private void Awake()
    {
        m_ui = GetComponentInChildren<InventoryUIController>();
        m_selector = GetComponent<Selector>();
        m_animatingCollectables = new HashSet<ICollectable>();
        m_actionAsset.OnInventoryEvent += HandleInventoryEvent;
        m_actionAsset.OnInteractEvent += HandleInteractEvent;
    }

    private void HandleInteractEvent()
    {
        ICollectable collectable = m_selector.GetCollectables()
            .FirstOrDefault(collectable => !m_animatingCollectables.Contains(collectable));

        if (collectable != null)
        {
            StartCoroutine(Collect(m_selector.GetCollectableTransform(collectable), collectable, 1f));
        }

        var interactable = m_selector.GetIntractable().FirstOrDefault();
        interactable?.Interact(transform);
    }

    private void HandleInventoryEvent()
    {
        m_ui.Display(!m_ui.IsDisplaying);
        m_actionAsset.IsPaused = !m_actionAsset.IsPaused;
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private IEnumerator Collect(Transform t, ICollectable c, float time)
    {
        m_playerAnimator.PlayUse();
        m_animatingCollectables.Add(c);
        m_collectStartSound.Play();
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
        m_animatingCollectables.Remove(c);
        m_collectSound.Play();

        yield return null;
    }
}

