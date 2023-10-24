using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;

public class Ship : MonoBehaviour, IInteractable, ISelectable
{
    [SerializeField] private List<ItemData> m_requiredItems;

    private List<ItemData> m_components;

    public void Interact(Transform _user)
    {
        throw new System.NotImplementedException();
    }

    public void Select()
    {
    }

    public void DeSelect()
    {
    }
}
