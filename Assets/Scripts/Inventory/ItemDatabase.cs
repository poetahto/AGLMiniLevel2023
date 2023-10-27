using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace InventorySystem
{
    public class ItemDatabase : ScriptableObject
    {
        private static ItemDatabase m_instance;

        private void OnEnable()
        {
            if (m_instance == null)
                m_instance = this;
        }

        [SerializeField] private List<ItemData> m_items;

        public static void AddItem(ItemData _data) => m_instance.AddItemImpl(_data);
        private void AddItemImpl(ItemData _data) => m_items.Add(_data);

        public static bool Contains(ItemData _data) => m_instance.ContainsImpl(_data);
        private bool ContainsImpl(ItemData _data) => m_items.Contains(_data);
        
        public static bool Contains(ItemID _id) => m_instance.ContainsImpl(_id); 
        private bool ContainsImpl(ItemID _id) => m_items.Any(item => item.ID == _id);

        private bool ContainsImpl(string _name) => m_items.Any(item => item.Name == _name);

        private static ItemData GetItemByName(string _name) => m_instance.GetItemByNameImpl(_name);
        private ItemData GetItemByNameImpl(string _name) => m_items.Find(item => item.Name == _name);

        private static ItemData GetItemByID(ItemID _id) => m_instance.GetItemByIDImpl(_id);
        private ItemData GetItemByIDImpl(ItemID _id) => m_items.Find(item => item.ID == _id);
    }
}
