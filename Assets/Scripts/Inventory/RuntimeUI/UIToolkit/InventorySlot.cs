using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace InventorySystem
{
    public sealed class InventorySlot : VisualElement
    {
        public Image Icon { get; }
        public Label Stack { get; }
        public InventoryItem Item { get; private set; }
        
        public InventorySlot()
        {
            Icon = new Image();
            Stack = new Label("0");
            Add(Icon);
            Add(Stack);
            
            Icon.AddToClassList("slot-icon");
            Stack.AddToClassList("stack-label");
            Stack.visible = false;
            AddToClassList("inventory-slot-container");
            
            RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if(evt.button != 0 || isEmpty()) return;
            
            Icon.sprite = null;
            Stack.visible = false;

            InventoryUIController.StartDragging(evt.position, this);
        }

        public void SetItem(InventoryItem _item)
        {
            Icon.sprite = _item.Data.Icon;
            Item = _item;
            Stack.text = _item.StackSize.ToString();
            Stack.visible = true;
        }
        
        public void ClearItem()
        {
            Icon.image = null;
            Item = null;
            Stack.visible = false;
        }

        public bool isEmpty()
        {
            return Item == null && Icon.sprite == null;
        }

        #region UXML
        [Preserve]
        public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> {}
        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits {}
        #endregion
    }
}
