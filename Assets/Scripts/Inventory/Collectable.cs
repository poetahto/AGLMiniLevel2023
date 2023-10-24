using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Collectable : MonoBehaviour, ICollectable
    {
        [SerializeField] private ItemData m_data;
        public void SetData(ItemData _data) => m_data = _data;
        
        public void Collect(Inventory _inventory)
        {
            _inventory.Add(m_data);
            Destroy(gameObject);
        }
    }
}
