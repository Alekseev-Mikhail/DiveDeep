using System;
using NUnit.Framework;
using Source.Utilities;
using Source.Utilities.Inventory;
using Tests.EditMode.Dummies;

namespace Tests.EditMode
{
    [TestFixture]
    public class InventoryTests
    {
        [Test]
        public void Init_InvalidSize_ThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Inventory<DummyItem>(-1, 10));
        }
        
        [Test]
        public void PutItem_InCertainPlace_PutsItem()
        {
            // Arrange
            var inventory = new Inventory<DummyItem>(10, 10);
            var item1 = new DummyItem(1, 1);
            var item2 = new DummyItem(1, 1);

            // Act
            var res1 = inventory.PutItem(item1, 0, 0);
            var res2 = inventory.PutItem(item2, 1, 0);

            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(true, res2);
            Assert.AreEqual(2, inventory.GetItems().Count);
            Assert.AreEqual(true, inventory.GetItems().ContainsKey(item1));
            Assert.AreEqual(true, inventory.GetItems().ContainsKey(item2));
        }
        
        [Test]
        public void PutItem_TwiceOneItem_PutsOnce()
        {
            // Arrange
            var inventory = new Inventory<DummyItem>(10, 10);
            var item = new DummyItem(2, 2);

            // Act
            var res1 = inventory.PutItem(item, 0, 0);
            var res2 = inventory.PutItem(item, 4, 4);

            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(false, res2);
            Assert.AreEqual(1, inventory.GetItems().Count);
            Assert.AreEqual(true, inventory.GetItems().ContainsKey(item));
        }
        
        [Test]
        public void PutItem_ItemOverlapping_PutsOnlyOneItem()
        {
            // Arrange
            var inventory = new Inventory<DummyItem>(10, 10);
            var item1 = new DummyItem(2, 2);
            var item2 = new DummyItem(2, 2);
            var item3 = new DummyItem(2, 2);

            // Act
            var res1 = inventory.PutItem(item1, 0, 0);
            var res2 = inventory.PutItem(item2, 0, 0);
            var res3 = inventory.PutItem(item3, 1, 1);

            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(false, res2);
            Assert.AreEqual(false, res3);
            Assert.AreEqual(1, inventory.GetItems().Count);
            Assert.AreEqual(true, inventory.GetItems().ContainsKey(item1));
        }
        
        [Test]
        public void PutItem_InvalidPlace_DoesNotPut()
        {
            // Arrange
            var inventory = new Inventory<DummyItem>(10, 10);
            var item1 = new DummyItem(1, 1);
            var item2 = new DummyItem(1, 1);

            // Act
            var res1 = inventory.PutItem(item1, -1, 0);
            var res2 = inventory.PutItem(item2, 10, 0);

            // Assert
            Assert.AreEqual(false, res1);
            Assert.AreEqual(false, res2);
            Assert.AreEqual(0, inventory.GetItems().Count);
        }
        
        [Test]
        public void MoveItem_ValidPlace_MovesItem()
        {
            // Arrange
            var inventory = new Inventory<DummyItem>(10, 10);
            var item = new DummyItem(1, 1);

            // Act
            var res1 = inventory.PutItem(item, 0, 0);
            var res2 = inventory.MoveItem(item, 1, 1);

            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(true, res2);
            Assert.AreEqual(1, inventory.GetItems()[item].x);
            Assert.AreEqual(1, inventory.GetItems()[item].y);
            Assert.AreEqual(1, inventory.GetItems().Count);
        }
        
        [Test]
        public void MoveItem_SameSpace_MovesItem()
        {
            // Arrange
            var inventory = new Inventory<DummyItem>(10, 10);
            var item = new DummyItem(2, 2);

            // Act
            var res1 = inventory.PutItem(item, 0, 0);
            var res2 = inventory.MoveItem(item, 1, 1);
            var res3 = inventory.MoveItem(item, 0, 0);

            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(true, res2);
            Assert.AreEqual(true, res3);
            Assert.AreEqual(0, inventory.GetItems()[item].x);
            Assert.AreEqual(0, inventory.GetItems()[item].y);
            Assert.AreEqual(1, inventory.GetItems().Count);
        }
        
        [Test]
        public void MoveItem_InvalidPlace_DoesNotMoveItem()
        {
            // Arrange
            var inventory = new Inventory<DummyItem>(10, 10);
            var item = new DummyItem(1, 1);

            // Act
            var res1 = inventory.PutItem(item, 0, 0);
            var res2 = inventory.MoveItem(item, 10, 0);
            var res3 = inventory.MoveItem(item, 0, 10);
            var res4 = inventory.MoveItem(item, -1, 0);
            var res5 = inventory.MoveItem(item, 0, -1);

            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(false, res2);
            Assert.AreEqual(false, res3);
            Assert.AreEqual(false, res4);
            Assert.AreEqual(false, res5);
            Assert.AreEqual(0, inventory.GetItems()[item].x);
            Assert.AreEqual(0, inventory.GetItems()[item].y);
            Assert.AreEqual(1, inventory.GetItems().Count);
        }
        
        [Test]
        public void MoveItem_SpaceIsNotFree_DoesNotMoveItem()
        {
            // Arrange
            var inventory = new Inventory<DummyItem>(10, 10);
            var item1 = new DummyItem(1, 1);
            var item2 = new DummyItem(1, 1);

            // Act
            var res1 = inventory.PutItem(item1, 0, 0);
            var res2 = inventory.PutItem(item2, 1, 0);
            var res3 = inventory.MoveItem(item1, 1, 0);

            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(true, res2);
            Assert.AreEqual(false, res3);
            Assert.AreEqual(0, inventory.GetItems()[item1].x);
            Assert.AreEqual(0, inventory.GetItems()[item1].y);
            Assert.AreEqual(1, inventory.GetItems()[item2].x);
            Assert.AreEqual(0, inventory.GetItems()[item2].y);
            Assert.AreEqual(2, inventory.GetItems().Count);
        }
    }
}