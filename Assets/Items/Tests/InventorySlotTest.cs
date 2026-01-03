using Items.Scripts;
using NUnit.Framework;

namespace Items.Tests
{
    public class InventorySlotTest
    {
        [Test]
        public void CreateInventorySlot_ShouldBeEmpty()
        {
            var slot = new InventorySlot();
            
            Assert.IsTrue(slot.IsEmpty);
            Assert.IsNull(slot.Item);
        }
        
        [Test]
        public void SetItemWithNull_ShouldSetSlotToEmpty()
        {
            var slot = new InventorySlot();
            
            slot.Set(null);
            
            Assert.IsTrue(slot.IsEmpty);    
            Assert.IsNull(slot.Item);
        }
        
        [Test]
        public void SetItemWithNullItemDefinition_ShouldSetSlotToEmpty()
        {
            var slot = new InventorySlot();
            
            slot.Set(new ItemInstance(null, 0));
            
            Assert.IsTrue(slot.IsEmpty);    
            Assert.IsNull(slot.Item);       
        }
        
        [Test]
        public void SetItemWithZeroQuantity_ShouldSetSlotToEmpty()
        {
            var slot = new InventorySlot();
            
            slot.Set(new ItemInstance(MaterialDefinition.NewBuilder().Build(), 0));
            
            Assert.IsTrue(slot.IsEmpty);    
            Assert.IsNull(slot.Item);      
        }
        
        [Test]
        public void SetItemWithNonZeroQuantity_ShouldSetSlotToNonEmpty()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            var quantityInSlot = 1;
            
            var slot = new InventorySlot();
            slot.Set(new ItemInstance(ore, quantityInSlot));
            
            Assert.IsFalse(slot.IsEmpty);    
            Assert.IsNotNull(slot.Item);      
            Assert.AreEqual(ore, slot.Item.Definition);
            Assert.AreEqual(quantityInSlot, slot.Item.Quantity);
        }

        [Test]
        public void ClearSlot_ShouldSetSlotToEmpty()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            
            var slot = new InventorySlot();
            slot.Set(new ItemInstance(ore, 1));
            
            slot.Clear();
            
            Assert.IsTrue(slot.IsEmpty);    
            Assert.IsNull(slot.Item);
        }
    }
}