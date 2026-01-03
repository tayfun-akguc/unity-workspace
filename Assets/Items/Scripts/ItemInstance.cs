using System;
using UnityEngine;

namespace Items.Scripts
{
    [Serializable]
    public class ItemInstance
    {
        [SerializeField] private ItemDefinition definition;
        [SerializeField] private int quantity = 1;

        public ItemDefinition Definition => definition;
        public int Quantity => quantity;

        public bool IsEmpty => definition == null || quantity <= 0;
        public int MaxStack => definition != null ? definition.MaxStack : 0;

        public ItemInstance(ItemDefinition definition, int quantity)
        {
            this.definition = definition;
            this.quantity = quantity;
            ClampQuantity();
        }

        public void SetQuantity(int newQuantity)
        {
            quantity = newQuantity;
            ClampQuantity();
        }

        public void AddQuantity(int amount)
        {
            quantity += amount;
            ClampQuantity();
        }

        public void RemoveQuantity(int amount)
        {
            quantity -= amount;
            ClampQuantity();
        }

        private void ClampQuantity()
        {
            if (definition == null)
            {
                quantity = 0;
                return;
            }

            if (quantity < 0) quantity = 0;
            if (quantity > definition.MaxStack) quantity = definition.MaxStack;
        }

        public bool CanStackWith(ItemInstance other)
        {
            if (definition == null) return false;
            if (other == null) return false;
            if (other.definition == null) return false;
            if (definition.GetType() != other.definition.GetType()) return false;
            if (definition.Id != other.definition.Id) return false;

            return quantity + other.Quantity <= definition.MaxStack;
        }

        public ItemInstance Clone(int cloneQuantity)
        {
            return new ItemInstance(definition, cloneQuantity);
        }
    }
}