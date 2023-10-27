using System;
using UnityEngine;

namespace InventorySystem
{
    /// <summary>
    /// A wrapper around the ItemData scriptableObject class and it represents an item within an inventory.
    /// It holds the data of the item it wraps an the stack size of the item (how many items of this type are in the inventory).
    /// </summary>
    [Serializable]
    public class InventoryItem
    {
        [SerializeField] private ItemData m_data;
        [SerializeField] private uint m_stackSize;
        public ItemData Data => m_data;
        public uint StackSize => m_stackSize;

        public InventoryItem(ItemData _data)
        {
            m_data = _data;
            IncreaseStack();
        }
        
        public InventoryItem(ItemData _data, uint _stackSize)
        {
            m_data = _data;
            m_stackSize = _stackSize;
        }

        public void IncreaseStack() => m_stackSize++;

        public void DecreaseStack() => m_stackSize--;

    }
}
