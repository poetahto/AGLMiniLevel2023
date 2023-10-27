using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace InventorySystem.Editor
{
    public class ItemDataEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_document;
        [SerializeField] private StyleSheet m_styles;
        
        [MenuItem("Tools/ItemData Editor")]
        public static void ShowWindow()
        {
            var window = CreateWindow<ItemDataEditorWindow>();
            window.titleContent = new GUIContent("ItemData Editor");
            window.minSize = new Vector2(800, 800);
        }

        private void OnEnable()
        {
            var treeAsset = m_document.CloneTree();
            rootVisualElement.Add(treeAsset);
            rootVisualElement.styleSheets.Add(m_styles);
            
            CreateListView();
        }

        private void CreateListView()
        {
            var items = FindAllItems();

            var itemList = rootVisualElement.Query<ListView>("itemData-list").First();
            itemList.makeItem = () =>
            {
                var label = new Label();
                label.styleSheets.Add(m_styles);
                label.AddToClassList("list-item");
                return label;
            };
            itemList.bindItem = (element, i) => ((Label)element).text = items[i].Name;

            itemList.itemsSource = items;
            itemList.fixedItemHeight = 20;
            itemList.selectionType = SelectionType.Single;

            itemList.selectionChanged += ItemSelectionChange;
        }

        private void ItemSelectionChange(IEnumerable<object> _enumerable)
        {
            foreach (var obj in _enumerable)
            {
                var itemInfo = rootVisualElement.Query<Box>("itemData-info").First();
                itemInfo.Clear();

                var item = obj as ItemData;

                var serializedObject = new SerializedObject(item);
                var property = serializedObject.GetIterator();
                property.Next(true);
                    
                while (property.NextVisible(false))
                {
                    var propField = new PropertyField(property);
                        
                    propField.SetEnabled(property.name != "m_Script");
                    propField.Bind(serializedObject);
                    itemInfo.Add(propField);

                    if (property.name == "Icon")
                    {
                        propField.RegisterCallback<ChangeEvent<Object>>(e => LoadItemIcon(item));
                    }
                        
                    LoadItemIcon(item);
                }
            }
        }

        private static ItemData[] FindAllItems()
        {
            var guids = AssetDatabase.FindAssets("t:ItemData");

            var items = new ItemData[guids.Length];

            for (int i = 0; i < items.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                items[i] = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            }

            return items;
        }

        private void LoadItemIcon(ItemData _data)
        {
            var iconPreview = rootVisualElement.Q<Image>("icon-preview");
            
            if (_data.Icon == null)
            {
                iconPreview.image = null;
                return;
            }
            
            iconPreview.image = _data.Icon.texture;
        }
    }
}
