using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class ItemID
    {
        private readonly Hash128[] m_hash;

        public ItemID(ItemData _item)
        {
            m_hash = new Hash128[2];

            m_hash[0] = Hash128.Compute(_item.Name);
            m_hash[1] = Hash128.Compute(_item.Description);
        }
        
        protected bool Equals(ItemID other)
        {
            return Equals(m_hash, other.m_hash);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
        
            return obj.GetType() == this.GetType() && Equals((ItemID)obj);
        }

        public override int GetHashCode()
        {
            return (m_hash != null ? m_hash.GetHashCode() : 0);
        }

        public bool Compare(ItemID _other)
        {
            return m_hash[0] == _other.m_hash[0] && m_hash[1] == _other.m_hash[1];
        }

        public static bool operator ==(ItemID _lhs, ItemID _rhs) => _lhs!.Compare(_rhs);

        public static bool operator !=(ItemID _lhs, ItemID _rhs) => !(_lhs == _rhs);
    }
}