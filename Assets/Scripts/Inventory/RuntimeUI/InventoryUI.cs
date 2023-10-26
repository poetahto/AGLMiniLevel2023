using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private Transform m_parent;
        [SerializeField] private GameObject m_uiPrefab;

        public void UpdateUI(IEnumerable<ItemRequirement> obj)
        {
            foreach (Transform item in m_parent)
            {
                Destroy(item.gameObject);
            }

            ReInitInventoryUI(obj);
        }

        private void ReInitInventoryUI(IEnumerable<ItemRequirement> inventory)
        {
            foreach (var item in inventory)
            {
                CreateInventoryItemSlot(item);
            }
        }

        private void CreateInventoryItemSlot(ItemRequirement item)
        {
            var obj = Instantiate(m_uiPrefab, m_parent, false);
            var ui = obj.GetComponent<UIItem>();
            ui.SetItem(new InventoryItem(item.Item, (uint)item.Amount));
        }
    }
}
