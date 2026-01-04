using UnityEngine;

namespace Items.Scripts
{
    public class InventoryTester : MonoBehaviour
    {
        public InventoryComponent inventory;
        public ItemDefinition itemA;
        public ItemDefinition itemB;

        [ContextMenu("Add 120 of ItemA")]
        private void AddA()
        {
            var added = inventory.Inventory.TryAdd(itemA, 120);
            Debug.Log($"Added {added} of {itemA.DisplayName}");
            PrintFirstSlots();
        }

        [ContextMenu("Add 3 of ItemB")]
        private void AddB()
        {
            int added = inventory.Inventory.TryAdd(itemB, 3);
            Debug.Log($"Added {added} of {itemB.DisplayName}");
            PrintFirstSlots();
        }

        [ContextMenu("Move Slot 0 -> Slot 1")]
        private void Move0to1()
        {
            inventory.Inventory.TryMove(0, 1);
            PrintFirstSlots();
        }

        [ContextMenu("Split Slot 0 (take 10)")]
        private void Split0()
        {
            int newIndex = inventory.Inventory.TrySplit(0, 10);
            Debug.Log($"Split result new slot index: {newIndex}");
            PrintFirstSlots();
        }

        private void PrintFirstSlots()
        {
            for (int i = 0; i < Mathf.Min(10, inventory.Inventory.Size); i++)
            {
                var slot = inventory.Inventory.GetSlot(i);
                if (slot.IsEmpty)
                    Debug.Log($"[{i}] EMPTY");
                else
                    Debug.Log($"[{i}] {slot.Item.Definition.DisplayName} x{slot.Item.Quantity}");
            }
        }
    }
}