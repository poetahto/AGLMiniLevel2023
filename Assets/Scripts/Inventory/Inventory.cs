using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Utilities;
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

        [SerializeField] private int m_maxCapacity;
        
        private readonly List<InventoryItem> InventoryItems = new();

        private int m_index = -1;
        public InventoryItem Current => InventoryItems[m_index];

        object IEnumerator.Current => Current;

        public int MaxCapacity => m_maxCapacity;
        
        public InventoryItem GetItem(ItemData _data) =>
            InventoryItems.FirstOrDefault(item => item.Data == _data);

        public bool Contains(ItemData _data) => InventoryItems.FirstOrDefault(item => item.Data == _data) != null;

        public void Add(ItemData _data)
        {
            if (InventoryItems.TryGet(_data, out var value))
            {
                value.IncreaseStack();
                OnInventoryChanged?.Invoke(value, InventoryChangeType.PickUp);
            }
            else
            {
                var item = new InventoryItem(_data);
                InventoryItems.Add(item);
                OnInventoryChanged?.Invoke(item, InventoryChangeType.PickUp);
            }
        }

        public void Remove(ItemData _data)
        {
            if (!InventoryItems.TryGet(_data, out var value)) return;

            value.DecreaseStack();

            OnInventoryChanged?.Invoke(value, InventoryChangeType.Drop);
            
            if (value.StackSize != 0) return;
            
            InventoryItems.Remove(value);
        }

        public void Remove(ItemData _data, int _amount)
        {
            for (var i = 0; i < _amount; i++)
            {
                Remove(_data);
            }
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
