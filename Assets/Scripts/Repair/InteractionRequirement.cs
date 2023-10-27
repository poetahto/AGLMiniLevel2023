using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using UnityEngine;

public class InteractionRequirement : MonoBehaviour
{
    public event Action OnRequirementFulFilled;
    
    [Tooltip("Should the item(s) be removed from the inventory if the requirement is fulfilled.")]
    [SerializeField] private bool m_shouldRemove;
    [SerializeField] private ItemRequirement[] m_requirements;

    public int NumRequirements => m_requirements.Length;
    public int CompletedRequirements { get; private set; }

    public bool IsCompleted => m_requirements.All(r => r.FulFilled);

    public IEnumerable<ItemRequirement> Requirements => m_requirements;
    
    public bool Evaluate(Inventory _inventory)
    {
        foreach (var requirement in m_requirements)
        {
            if (requirement.FulFilled) continue;

            if (requirement.Evaluate(_inventory))
            {
                if (m_shouldRemove)
                    _inventory.Remove(requirement.Item, requirement.Amount);

                CompletedRequirements++;
                
                OnRequirementFulFilled?.Invoke();
            }
            else
            {
                return false;
            }
        }
        
        return true;
    }
}
