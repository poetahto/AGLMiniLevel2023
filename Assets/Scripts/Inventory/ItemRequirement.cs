using System;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class ItemRequirement
    {
        [Tooltip("Should the item(s) be removed from the inventory if the requirement is fulfilled.")]
        [SerializeField] private bool m_shouldRemove;
        [SerializeField] private ItemData m_item;
        [SerializeField] private int m_amount;

        public bool FulFilled { get; private set; }
        
        public bool Evaluate(Inventory _inventory)
        {
            if (!_inventory.Contains(m_item)) return false;

            var item = _inventory.GetItem(m_item);

            if (item.StackSize >= m_amount)
            {
                FulFilled = true;
                if (!m_shouldRemove) return true;
                
                for (var i = 0; i < m_amount; i++)
                {
                    _inventory.Remove(m_item);
                }
            }
            else
            {
                FulFilled = false;
                return false;
            }

            FulFilled = true;
            return true;
        }
    }
}