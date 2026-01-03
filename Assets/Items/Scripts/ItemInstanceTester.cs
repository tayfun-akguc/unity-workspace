using UnityEngine;

namespace Items.Scripts
{
    public class ItemInstanceTester: MonoBehaviour
    {
        public ItemDefinition testDefinition;
        public InventorySlot slot = new InventorySlot();
        
        [ContextMenu("Put 5 Into Slot")]
        private void PutFive()
        {
            slot.Set(new ItemInstance(testDefinition, 5));
            Debug.Log($"Slot now: {slot.GetName()} x{slot.GetQuantity()}");
        }
        
        [ContextMenu("Add 3")]
        private void AddThree()
        {
            if (slot.IsEmpty)
            {
                slot.Set(new ItemInstance(testDefinition, 3));
            }
            else
            {
                slot.Item.AddQuantity(3);
            }
            Debug.Log($"Slot now: {slot.GetName()} x{slot.GetQuantity()}");
        }
        
        [ContextMenu("Clear Slot")]
        private void ClearSlot()
        {
            slot.Clear();
            Debug.Log("Slot cleared");
        }
    }
}