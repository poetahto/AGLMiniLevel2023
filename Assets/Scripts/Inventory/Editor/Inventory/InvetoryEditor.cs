using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace InventorySystem.Editor
{
    [CustomEditor(typeof(Inventory))]
    public class InventoryEditor : UnityEditor.Editor
    {
        public static bool OpenEditorWindow(int instanceID, int line)
        {
            var target = EditorUtility.InstanceIDToObject(instanceID) as Inventory;
            
            var isWindowOpen = EditorWindow.HasOpenInstances<InventoryEditorWindow>();
            if (!isWindowOpen)
            {
                InventoryEditorWindow.OpenWindow(target);
                return true;
            }
            
            EditorWindow.FocusWindowIfItsOpen<InventoryEditorWindow>();
            return false;
        }

        public override VisualElement CreateInspectorGUI()
        {
            return base.CreateInspectorGUI();
        }
    }
}
