using Items.Scripts;
using NUnit.Framework;
using UnityEngine;

namespace Items.Tests
{
    public class ItemInstanceTest
    {
        [Test]
        public void CreateItemInstance()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            
            var oreItemInstance = new ItemInstance(ore, 10);
            
            Assert.AreEqual(ore, oreItemInstance.Definition);
            Assert.AreEqual(10, oreItemInstance.Quantity);
        }

        [Test]
        public void SetQuantity_ShouldSetQuantityAndClamp()
        {
            const int maxStack = 999;
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(maxStack)
                .Build();
            
            var oreItemInstance = new ItemInstance(ore, 1);
            
            var quantityToSet = maxStack * 2;
            oreItemInstance.SetQuantity(quantityToSet);
            
            Assert.AreEqual(ore, oreItemInstance.Definition);
            Assert.AreEqual(maxStack, oreItemInstance.Quantity);
            Assert.AreNotEqual(quantityToSet, oreItemInstance.Quantity);
        }

        [Test]
        public void AddQuantity_ShouldAddQuantityAndClamp()
        {
            const int maxStack = 999;
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(maxStack)
                .Build();
            
            var oreItemInstance = new ItemInstance(ore, 1);
            var quantityToAdd = maxStack * 2;
            oreItemInstance.AddQuantity(quantityToAdd);
            
            Assert.AreEqual(ore, oreItemInstance.Definition);
            Assert.AreEqual(maxStack, oreItemInstance.Quantity);
            Assert.AreNotEqual(quantityToAdd, oreItemInstance.Quantity);
        }
        
        [Test]
        public void RemoveQuantity_ShouldRemoveQuantityAndClamp()
        {
            const int maxStack = 999;
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(maxStack)
                .Build();
            
            var oreItemInstance = new ItemInstance(ore, maxStack);
            var quantityToRemove = maxStack * 2;
            oreItemInstance.RemoveQuantity(quantityToRemove);
            
            Assert.AreEqual(ore, oreItemInstance.Definition);
            Assert.AreEqual(0, oreItemInstance.Quantity);
        }
        
        [Test]
        public void CanStackWith_ShouldReturnFalseIfBothItemDefinitionsAreNull()
        {
            var itemInstance = new ItemInstance(null, 0);  
            var otherItemInstance = new ItemInstance(null, 0); 
            Assert.IsFalse(itemInstance.CanStackWith(otherItemInstance));
        }

        [Test]
        public void CanStackWith_ShouldReturnFalseIfOtherItemInstanceIsNull()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            
            var oreItemInstance = new ItemInstance(ore, 10);   
            
            Assert.IsFalse(oreItemInstance.CanStackWith(null));
        }

        [Test]
        public void CanStackWith_ShouldReturnFalseIfOtherItemDefinitionIsNull()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            
            var oreItemInstance = new ItemInstance(ore, 10);   
            var otherItemInstance = new ItemInstance(null, 0);
            
            Assert.IsFalse(oreItemInstance.CanStackWith(otherItemInstance));
        }

        [Test]
        public void CanStackWith_ShouldReturnFalseIfItemDefinitionsAreDifferent()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            
            var oreItemInstance = new ItemInstance(ore, 10);

            var chest = WearableDefinition.NewBuilder()
                .Id("chest")
                .DisplayName("Chest")
                .Category(ItemCategory.Wearable)
                .MaxStack(999)
                .EquipSlot(EquipSlot.Chest)
                .Build();
            
            var chestItemInstance = new ItemInstance(chest, 10);
            
            Assert.IsFalse(oreItemInstance.CanStackWith(chestItemInstance));
        }

        [Test]
        public void CanStackWith_ShouldReturnFalseIfIdsAreDifferent()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            var oreItemInstance = new ItemInstance(ore, 10);
            
            var otherOre = MaterialDefinition.NewBuilder()
                .Id("other-ore")
                .DisplayName("Other Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            var otherOreItemInstance = new ItemInstance(otherOre, 10);
            
            Assert.IsFalse(oreItemInstance.CanStackWith(otherOreItemInstance));
        }

        [Test]
        public void CanStackWith_ShouldReturnFalseIfStackOverflow()
        {
            const int maxStack = 1;
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(maxStack)
                .Build();
            
            var oreItemInstance = new ItemInstance(ore, maxStack);
            var otherItemInstance = new ItemInstance(ore, maxStack);
            
            Assert.IsFalse(oreItemInstance.CanStackWith(otherItemInstance));
        }
        
        [Test]
        public void CanStackWith_ShouldReturnTrueIfStackable()
        {
            const int maxStack = 999;
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(maxStack)
                .Build();
            
            var itemInstance = new ItemInstance(ore, 10);
            var otherItemInstance = new ItemInstance(ore, 10);
            
            Assert.IsTrue(itemInstance.CanStackWith(otherItemInstance));
        }
        
        [Test]
        public void Clone_ShouldCloneItemInstance()
        {
            var maxStack = 999;
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(maxStack)
                .Build();
            
            var itemInstance = new ItemInstance(ore, maxStack);
            var clonedItemInstance = itemInstance.Clone(10);
            
            Assert.AreEqual(itemInstance.Definition, clonedItemInstance.Definition);
            Assert.AreEqual(10, clonedItemInstance.Quantity);
        }
    }
}