using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ItemObject : MonoBehaviour, ICollectable
    {
        [SerializeField] private ItemData m_data;
        [SerializeField] private int m_stackSize;

        public int StackSize { get; set; }

        private void Awake()
        {
            StackSize = m_stackSize;
        }

        public void SetData(ItemData _data) => m_data = _data;

        public GameObject Object => m_data.Prefab;

        public void Select()
        {

        }

        public void DeSelect()
        {

        }

        public void Interact(Transform _user)
        {
        }

        public InventoryItem Collect(Inventory _inventory)
        {
            for (var i = 0; i < StackSize; i++)
            {
                _inventory.Add(m_data);
            }

            return _inventory.GetItem(m_data);
        }
    }
}
