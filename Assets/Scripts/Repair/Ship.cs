using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour, IInteractable, ISelectable
{
    [SerializeField] private InteractionRequirement m_interactionRequirement;
    [SerializeField] private ProgressBar m_progressBar;
    [SerializeField] private InventoryUI m_inventoryUI;

    private void OnEnable()
    {
        m_interactionRequirement.OnRequirementFulFilled += RequirementFulfilled;
    }
    
    private void OnDisable()
    {
        m_interactionRequirement.OnRequirementFulFilled += RequirementFulfilled;
    }

    private void Start()
    {
        RequirementFulfilled();
        DeSelect();
    }

    private void RequirementFulfilled()
    {
        m_inventoryUI.UpdateUI(m_interactionRequirement.Requirements.Where(r => !r.FulFilled));
        m_progressBar.Fill = (float)m_interactionRequirement.CompletedRequirements / m_interactionRequirement.NumRequirements;
    }

    public void Interact(Transform _user)
    {
        if (!_user.TryGetComponent<PlayerInventoryController>(out var inventoryController)) return;
        
        if (m_interactionRequirement.Evaluate(inventoryController.PlayerInventory))
        {
            Debug.Log("Ship repaired!!");
        }
    }

    public void Select()
    {
        m_progressBar.gameObject.SetActive(true);
        
        if (!m_interactionRequirement.IsCompleted)
            m_inventoryUI.gameObject.SetActive(true);
    }

    public void DeSelect()
    {
        m_progressBar.gameObject.SetActive(false);
        m_inventoryUI.gameObject.SetActive(false);
    }
}
