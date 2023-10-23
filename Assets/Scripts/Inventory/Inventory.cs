using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public enum InventoryChangeType
    {
        PickUp,
        Drop,
    }
    
    [CreateAssetMenu(menuName = "Inventory System/Inventory", fileName = "New Inventory")]
    public class Inventory : ScriptableObject, IEnumerable<InventoryItem>, IEnumerator<InventoryItem>
    {
        public event Action<InventoryItem, InventoryChangeType> OnInventoryChanged;

        private readonly Dictionary<ItemData, InventoryItem> m_itemDictionary = new();

        [SerializeField] private int m_maxCapacity;
        [SerializeField] private List<InventoryItem> InventoryItems = new();

        private int m_index = -1;
        public InventoryItem Current => InventoryItems[m_index];

        object IEnumerator.Current => Current;

        public int MaxCapacity => m_maxCapacity;
        
        public InventoryItem GetItem(ItemData _data) =>
            m_itemDictionary.FirstOrDefault(item => item.Key == _data).Value;

        public bool Contains(ItemData _data) => m_itemDictionary.ContainsKey(_data);

        public void Add(ItemData _data)
        {
            if (m_itemDictionary.TryGetValue(_data, out var value))
            {
                value.IncreaseStack();
            }
            else
            {
                var item = new InventoryItem(_data);
                InventoryItems.Add(item);
                m_itemDictionary.Add(_data, item);
            }

            OnInventoryChanged?.Invoke(m_itemDictionary[_data], InventoryChangeType.PickUp);
        }

        public void Remove(ItemData _data)
        {
            if (!m_itemDictionary.TryGetValue(_data, out var value)) return;

            value.DecreaseStack();

            OnInventoryChanged?.Invoke(m_itemDictionary[_data], InventoryChangeType.Drop);
            
            if (value.StackSize != 0) return;
            
            InventoryItems.Remove(value);
            m_itemDictionary.Remove(_data);
        }

        public IEnumerator<InventoryItem> GetEnumerator()
        {
            return InventoryItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool MoveNext()
        {
            m_index++;
            return (m_index < InventoryItems.Count);
        }

        public void Reset()
        {
            m_index = -1;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
