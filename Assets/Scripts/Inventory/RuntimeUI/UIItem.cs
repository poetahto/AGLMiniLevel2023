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

        private string Name
        {
            set => m_name.text = value;
        }

        private Sprite Icon
        {
            set => m_icon.sprite = value;
        }

        private string StackSize
        {
            set => m_stack.text = value;
        }

        private void Awake()
        {
            m_icon = GetComponentInChildren<Image>();
            m_name = GetComponentInChildren<TextMeshProUGUI>();
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
