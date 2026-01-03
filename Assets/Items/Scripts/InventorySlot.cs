using System;
using UnityEngine;

namespace Items.Scripts
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField] private ItemInstance item;
        
        public ItemInstance Item => item;
        public bool IsEmpty => item == null || item.IsEmpty;
        
        public void Clear()
        {
            item = null;
        }
        
        public void Set(ItemInstance newItem)
        {
            item = newItem;
            if (item != null && item.IsEmpty)
            {
                item = null;
            }
        }
    }
}