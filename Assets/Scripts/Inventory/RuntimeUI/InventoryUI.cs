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

        private void UpdateInventory(Inventory obj)
        {
            foreach (Transform item in m_parent)
            {
                Destroy(item.gameObject);
            }

            ReInitInventoryUI(obj);
        }

        private void ReInitInventoryUI(Inventory inventory)
        {
            foreach (var item in inventory)
            {
                CreateInventoryItemSlot(item);
            }
        }

        private void CreateInventoryItemSlot(InventoryItem item)
        {
            var obj = Instantiate(m_uiPrefab, m_parent, false);
            var ui = obj.GetComponent<UIItem>();
            ui.SetItem(item);
        }
    }
}
