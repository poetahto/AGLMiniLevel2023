using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace InventorySystem.Editor
{
    public class InventoryEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_document;
        [SerializeField] private StyleSheet m_styles;
        
        private static SerializedObject serializedObject;

        private Button m_increase;
        private Button m_decrease;
        private Button m_delete;
        
        public static void OpenWindow(Inventory _context)
        {
            var window = CreateWindow<InventoryEditorWindow>();
            window.titleContent = new GUIContent($"{_context.name} Editor");
            window.minSize = new Vector2(800, 800);
            serializedObject = new SerializedObject(_context);
        }
        
        private void OnEnable()
        {
            var treeAsset = m_document.CloneTree();
            rootVisualElement.Add(treeAsset);
            rootVisualElement.styleSheets.Add(m_styles);

            m_increase = rootVisualElement.Query<Button>("plus").First();
            m_decrease = rootVisualElement.Query<Button>("minus").First();
            m_delete = rootVisualElement.Query<Button>("delete-button").First();
            
            RenderInventoryItems();
        }

        private void RenderInventoryItems()
        {
            var scrollView = rootVisualElement.Query<VisualElement>("unity-content-container").First();
            var list = serializedObject.FindProperty("InventoryItems");
            var it = list.GetEnumerator();
            while (it.MoveNext())
            {
                var item = it.Current as InventoryItem;

                var slot = new InventorySlot();
                slot.SetItem(item);
                
                scrollView.Add(slot);
            }
        }

    }
}
