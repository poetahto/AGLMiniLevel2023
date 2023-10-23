using System;
using UnityEngine;

namespace InventorySystem
{
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

        public void IncreaseStack() => m_stackSize++;

        public void DecreaseStack() => m_stackSize--;

    }
}
