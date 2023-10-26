using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class UIItem : MonoBehaviour
    {
        [SerializeField] private Image m_icon;
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private TextMeshProUGUI m_stack;

        private InventoryItem m_item;

        public string Name
        {
            set => m_name.text = value;
        }

        public Sprite Icon
        {
            set => m_icon.sprite = value;
        }

        public string StackSize
        {
            set => m_stack.text = value;
        }

        public void SetItem(InventoryItem _item)
        {
            m_item = _item;
            Name = m_item.Data.Name;
            Icon = m_item.Data.Icon;
            StackSize = m_item.StackSize.ToString();
        }
    }
}
