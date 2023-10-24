using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ItemObject : MonoBehaviour, ICollectable
    {
        [SerializeField] private ItemData m_data;
        public void SetData(ItemData _data) => m_data = _data;
        
        public InventoryItem Collect(Inventory _inventory)
        {
            _inventory.Add(m_data);
            Destroy(gameObject);
            return _inventory.GetItem(m_data);
        }
    }
}
