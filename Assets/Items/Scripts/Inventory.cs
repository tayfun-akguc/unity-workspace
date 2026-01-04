using System;
using UnityEngine;

namespace Items.Scripts
{
    [Serializable]
    public class Inventory
    {
        [SerializeField] private int size;
        [SerializeField] private InventorySlot[] slots;

        public int Size => slots != null ? slots.Length : 0;
        public InventorySlot[] Slots => slots;

        public Inventory(int size)
        {
            this.size = Math.Max(1, size);
            slots = new InventorySlot[this.size];
            for (var i = 0; i < slots.Length; i++)
            {
                slots[i] = new InventorySlot();
            }
        }

        public void InitializeIfNeeded()
        {
            if (slots != null && slots.Length == size) return;

            size = Math.Max(1, size);
            slots = new InventorySlot[size];
            for (var i = 0; i < slots.Length; i++)
            {
                slots[i] = new InventorySlot();
            }
        }

        public InventorySlot GetSlot(int index)
        {
            if (index < 0 || index >= slots.Length) return null;
            return slots[index];
        }

        // Returns how many were actually added
        public int TryAdd(ItemDefinition def, int amount)
        {
            if (def == null || amount <= 0) return 0;

            Debug.Log($"Adding {amount} of {def.DisplayName}");
            
            var remaining = amount;

            // 1) Fill existing stacks
            if (def.MaxStack > 1)
            {
                for (var i = 0; i < slots.Length && remaining > 0; i++)
                {
                    var slot = slots[i];
                    if (slot.IsEmpty) continue;

                    var item = slot.Item;
                    if (item.Definition != def) continue;

                    var space = def.MaxStack - item.Quantity;
                    if (space <= 0) continue;

                    var toAdd = Math.Min(space, remaining);
                    item.AddQuantity(toAdd);
                    remaining -= toAdd;
                }
            }

            // 2) Put into empty slots (create new stacks)
            for (var i = 0; i < slots.Length && remaining > 0; i++)
            {
                var slot = slots[i];
                if (!slot.IsEmpty) continue;

                var stackQty = Math.Min(def.MaxStack, remaining);
                slot.Set(new ItemInstance(def, stackQty));
                remaining -= stackQty;
            }

            return amount - remaining;
        }

        // Returns true if it removed the full requested amount
        public bool TryRemoveAt(int index, int amount)
        {
            if (amount <= 0) return true;

            var slot = GetSlot(index);
            if (slot == null || slot.IsEmpty) return false;

            var item = slot.Item;
            if (item.Quantity < amount) return false;

            item.RemoveQuantity(amount);
            if (item.Quantity <= 0) slot.Clear();
            return true;
        }

        public bool TryMove(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex) return false;

            var from = GetSlot(fromIndex);
            var to = GetSlot(toIndex);
            if (from == null || to == null) return false;
            if (from.IsEmpty) return false;

            // If destination empty => simple move
            if (to.IsEmpty)
            {
                to.Set(from.Item);
                from.Clear();
                return true;
            }

            // Try merge if same item & stackable
            var a = from.Item;
            var b = to.Item;

            if (a.Definition == b.Definition && a.Definition.MaxStack > 1)
            {
                var space = a.Definition.MaxStack - b.Quantity;
                if (space > 0)
                {
                    var moved = Math.Min(space, a.Quantity);
                    b.AddQuantity(moved);
                    a.RemoveQuantity(moved);

                    if (a.Quantity <= 0) from.Clear();
                    return true;
                }
            }

            // Otherwise swap
            var temp = to.Item;
            to.Set(from.Item);
            from.Set(temp);
            return true;
        }

        // Split stack: takes `amount` out of slotIndex and puts it into an empty slot.
        // Returns index of new slot, or -1 if failed.
        public int TrySplit(int slotIndex, int amount)
        {
            var slot = GetSlot(slotIndex);
            if (slot == null || slot.IsEmpty) return -1;

            var item = slot.Item;
            if (item.Definition.MaxStack <= 1) return -1; // cannot split non-stackable
            if (amount <= 0) return -1;
            if (amount >= item.Quantity) return -1; // must leave something behind

            int emptyIndex = FindFirstEmptySlot();
            if (emptyIndex == -1) return -1;

            // remove from original
            item.RemoveQuantity(amount);

            // create new stack
            slots[emptyIndex].Set(item.Clone(amount));
            return emptyIndex;
        }

        public int FindFirstEmptySlot()
        {
            for (var i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty) return i;
            }

            return -1;
        }
    }
}