using System;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class ItemRequirement
    {
        [SerializeField] private ItemData m_item;
        [SerializeField] private int m_amount;

        public bool FulFilled { get; private set; }

        public ItemData Item => m_item;
        public int Amount => m_amount;
        
        public bool Evaluate(Inventory _inventory)
        {
            if (!_inventory.Contains(m_item)) return false;

            var item = _inventory.GetItem(m_item);

            if (item.StackSize >= m_amount)
            {
                FulFilled = true;
                return true;
            }

            FulFilled = false;
            return false;
        }
    }
}