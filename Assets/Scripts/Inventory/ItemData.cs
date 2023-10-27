using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace InventorySystem
{
    /// <summary>
    /// Holds basic information about an item.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "Inventory System/Items/ItemData", fileName = "ItemData")]
    public class ItemData : ScriptableObject
    {
        private ItemID m_id;
        public ItemID ID => m_id ??= new ItemID(this);

        public string Name;
        [TextArea] public string Description;
        public Sprite Icon;
        public GameObject Prefab;
    }
}