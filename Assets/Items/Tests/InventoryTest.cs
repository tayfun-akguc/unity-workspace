using Items.Scripts;
using NUnit.Framework;

namespace Items.Tests
{
    public class InventoryTest
    {
        [Test]
        public void CreateInventoryWithNegativeSize_ShouldCreateWithOneSlot()
        {
            var inventory = new Inventory(-1);

            Assert.AreEqual(1, inventory.Size);
        }

        [Test]
        public void CreateInventory_ShouldCreateGivenSize()
        {
            var inventory = new Inventory(10);

            Assert.AreEqual(10, inventory.Size);
        }

        [Test]
        public void FindFirstEmptySlot_ShouldReturnFirstEmptySlot()
        {
            var inventory = new Inventory(10);
            var slot = inventory.FindFirstEmptySlot();

            Assert.AreEqual(0, slot);
        }

        [Test]
        public void TryAdd_ShouldReturnAddedCount()
        {
            const int oreQuantity = 10;
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();

            const int inventorySize = 10;
            var inventory = new Inventory(inventorySize);
            var addedCount = inventory.TryAdd(ore, oreQuantity);
            var inventorySlot = inventory.GetSlot(0);
            
            var firstEmptySlot = inventory.FindFirstEmptySlot();
            
            Assert.False(inventorySlot.IsEmpty);
            Assert.IsNotNull(inventorySlot.Item);
            Assert.AreNotEqual(firstEmptySlot, 0);
            Assert.AreEqual(firstEmptySlot, 1);
            Assert.AreEqual(ore, inventorySlot.Item.Definition);
            Assert.AreEqual(oreQuantity, inventorySlot.Item.Quantity);
            Assert.AreEqual(oreQuantity, addedCount);
        }

        [Test]
        public void TryAdd_ShouldCreateNewStackIfSlotIsFull()
        {
            const int oreQuantity = 20;
            const int oreMaxStack = 10;
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(oreMaxStack)
                .Build();
            
            var inventorySize = 10;
            var inventory = new Inventory(inventorySize);
            var addedCount = inventory.TryAdd(ore, oreQuantity);
            
            var firstEmptySlot = inventory.FindFirstEmptySlot();
            
            Assert.False(inventory.GetSlot(0).IsEmpty);
            Assert.AreEqual(oreMaxStack, inventory.GetSlot(0).Item.Quantity);
            Assert.False(inventory.GetSlot(1).IsEmpty);
            Assert.AreEqual(oreMaxStack, inventory.GetSlot(1).Item.Quantity);
            Assert.AreEqual(oreQuantity, addedCount);
            Assert.AreEqual(firstEmptySlot, 2);
        }
        
        [Test]
        public void TryAdd_ShouldReturnZeroIfInventoryIsFull()
        {
            const int oreQuantity = 10;
            const int oreMaxStack = 10;
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(10)
                .Build();
            
            var inventorySize = 1;
            var inventory = new Inventory(inventorySize);
            var addedCount = inventory.TryAdd(ore, oreQuantity);
            var reAddCount = inventory.TryAdd(ore, oreQuantity);
            
            Assert.AreEqual(oreMaxStack, addedCount);
            Assert.AreEqual(0, reAddCount);
        }
        
        [Test]
        public void TryAdd_ShouldReturnZeroIfAmountIsZero()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            
            var inventorySize = 10;
            var inventory = new Inventory(inventorySize);
            
            var addedCount = inventory.TryAdd(ore, 0);
            var firstEmptySlot = inventory.FindFirstEmptySlot();
            
            Assert.AreEqual(0, addedCount);
            Assert.AreEqual(firstEmptySlot, 0);
        }
        
        [Test]
        public void TryAdd_ShouldReturnZeroIfItemDefinitionIsNull()
        {
            var inventorySize = 10;
            var inventory = new Inventory(inventorySize);
            
            var addedCount = inventory.TryAdd(null, 10);
            var firstEmptySlot = inventory.FindFirstEmptySlot();
            
            Assert.AreEqual(0, addedCount);
            Assert.AreEqual(firstEmptySlot, 0);
        }

        [Test]
        public void TryRemoveAt_ShouldReturnTrueWhenAmountIsZero()
        {
            var inventorySize = 10;
            var inventory = new Inventory(inventorySize);
            
            var moved = inventory.TryRemoveAt(0, 0);
            
            Assert.True(moved);
        }

        [Test]
        public void TryRemoveAt_ShouldReturnFalseWhenSlotIsNull()
        {
            var inventorySize = 10;
            var inventory = new Inventory(inventorySize);
            
            var moved = inventory.TryRemoveAt(inventorySize, 1);
            
            Assert.False(moved);
        }

        [Test]
        public void TryRemoveAt_ShouldReturnTrueWhenAmountIsLessThanItemQuantity()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            var inventorySize = 10;
            var inventory = new Inventory(inventorySize);
            const int addCount = 10;
            inventory.TryAdd(ore, addCount);
            const int removeCount = 1;
            
            var moved = inventory.TryRemoveAt(0, removeCount);
            
            Assert.True(moved);
            Assert.AreEqual(addCount - removeCount, inventory.GetSlot(0).Item.Quantity);
        }

        [Test]
        public void TryRemoveAt_ShouldReturnFalseWhenAmountIsGreaterThanItemQuantity()
        {
            var ore = MaterialDefinition.NewBuilder()
                .Id("ore")
                .DisplayName("Ore")
                .Category(ItemCategory.Material)
                .MaxStack(999)
                .Build();
            var inventorySize = 10;
            var inventory = new Inventory(inventorySize);
            inventory.TryAdd(ore, 10);
            
            var moved = inventory.TryRemoveAt(0, 10 * 2);
            
            Assert.False(moved);
        }
    }
}