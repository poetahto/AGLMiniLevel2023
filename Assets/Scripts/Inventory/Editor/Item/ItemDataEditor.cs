using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace InventorySystem.Editor
{
    [CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            var it = serializedObject.GetIterator();
            it.Next(true);

            while (it.NextVisible(false))
            {
                var prop = new PropertyField(it);
                prop.SetEnabled(it.name != "m_Script");
                container.Add(prop);
            }

            return container;
        }
    }
}
