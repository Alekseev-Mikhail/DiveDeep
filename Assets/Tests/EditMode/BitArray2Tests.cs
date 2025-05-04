using NUnit.Framework;
using Source.Utilities;

namespace Tests.EditMode
{
    [TestFixture]
    public class BitArray2Tests
    {
        [Test]
        public void Init_2x4_ReturnsArray()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 2;
            const int height = 4;

            // Act
            var res = bitArray.Init(width, height);
            
            // Assert
            Assert.AreEqual(true, res);
            Assert.AreEqual(width, bitArray.GetWidth());
            Assert.AreEqual(height, bitArray.GetHeight());
            Assert.AreEqual(false, bitArray.GetValue(0, 0));
            Assert.AreEqual(false, bitArray.GetValue(width - 1, 1));
            Assert.AreEqual(false, bitArray.GetValue(width - 1, height - 1));
        }
        
        [Test]
        public void Init_10x10_ReturnsArray()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 10;
            const int height = 10;

            // Act
            var res = bitArray.Init(width, height);
            
            // Assert
            Assert.AreEqual(true, res);
            Assert.AreEqual(width, bitArray.GetWidth());
            Assert.AreEqual(height, bitArray.GetHeight());
            Assert.AreEqual(false, bitArray.GetValue(0, 0));
            Assert.AreEqual(false, bitArray.GetValue(width - 1, 1));
            Assert.AreEqual(false, bitArray.GetValue(width - 1, height - 1));
        }
        
        [Test]
        public void Init_InvalidSize_ReturnsFalse()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = -1;
            const int height = 10;

            // Act
            var res = bitArray.Init(width, height);
            
            // Assert
            Assert.AreEqual(false, res);
        }
        
        [Test]
        public void GetValue_InvalidXY_ReturnsZero()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 10;
            const int height = 10;
            
            // Act
            bitArray.Init(width, height);
            var res1 = bitArray.GetValue(-1, 0);
            var res2 = bitArray.GetValue(10, 0);
            
            // Assert
            Assert.AreEqual(false, res1);
            Assert.AreEqual(false, res2);
        }
        
        [Test]
        public void Invert_SeveralCells_SetsValueToOne()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 10;
            const int height = 10;
            
            // Act
            bitArray.Init(width, height);
            var res1 = bitArray.Invert(0, 0);
            var res2 = bitArray.Invert(2, 2);
            var res3 = bitArray.Invert(width - 1, height - 1);
            
            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(true, res2);
            Assert.AreEqual(true, res3);
            Assert.AreEqual(true, bitArray.GetValue(0, 0));
            Assert.AreEqual(true, bitArray.GetValue(2, 2));
            Assert.AreEqual(true, bitArray.GetValue(width - 1, height - 1));
        }
        
        [Test]
        public void Invert_InvalidXY_NotSetValue()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 10;
            const int height = 10;
            
            // Act
            bitArray.Init(width, height);
            var res1 = bitArray.Invert(-1, 0);
            var res2 = bitArray.Invert(10, 9);
            
            // Assert
            Assert.AreEqual(false, res1);
            Assert.AreEqual(false, res2);
        }
        
        [Test]
        public void Invert_SeveralCellsTwice_SetsValueToZero()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 10;
            const int height = 10;
            
            // Act
            bitArray.Init(width, height);
            var res1 = bitArray.Invert(0, 0);
            var res2 = bitArray.Invert(0, 0);
            var res3 = bitArray.Invert(2, 2);
            var res4 = bitArray.Invert(2, 2);
            var res5 = bitArray.Invert(width - 1, height - 1);
            var res6 = bitArray.Invert(width - 1, height - 1);
            
            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(true, res2);
            Assert.AreEqual(true, res3);
            Assert.AreEqual(true, res4);
            Assert.AreEqual(true, res5);
            Assert.AreEqual(true, res6);
            Assert.AreEqual(false, bitArray.GetValue(0, 0));
            Assert.AreEqual(false, bitArray.GetValue(2, 2));
            Assert.AreEqual(false, bitArray.GetValue(width - 1, height - 1));
        }
        
        [Test]
        public void InvertArea_ValidArea_SetsValuesToOne()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 10;
            const int height = 10;
            
            // Act
            bitArray.Init(width, height);
            var res = bitArray.InvertArea(0, 0, 2, 2);
            
            // Assert
            Assert.AreEqual(true, res);
            Assert.AreEqual(true, bitArray.GetValue(0, 0));
            Assert.AreEqual(true, bitArray.GetValue(1, 0));
            Assert.AreEqual(true, bitArray.GetValue(1, 1));
            Assert.AreEqual(true, bitArray.GetValue(0, 1));
        }
        
        [Test]
        public void InvertArea_ValidAreaTwice_SetsValuesToZero()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 10;
            const int height = 10;
            
            // Act
            bitArray.Init(width, height);
            var res1 = bitArray.InvertArea(0, 0, 2, 2);
            var res2 = bitArray.InvertArea(0, 0, 2, 2);
            
            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(true, res2);
            Assert.AreEqual(false, bitArray.GetValue(0, 0));
            Assert.AreEqual(false, bitArray.GetValue(1, 0));
            Assert.AreEqual(false, bitArray.GetValue(1, 1));
            Assert.AreEqual(false, bitArray.GetValue(0, 1));
        }
        
        [Test]
        public void InvertArea_AtEdge_SetsValuesToOne()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 10;
            const int height = 10;
            
            // Act
            bitArray.Init(width, height);
            var res = bitArray.InvertArea(8, 8, 2, 2);
            
            // Assert
            Assert.AreEqual(true, res);
            Assert.AreEqual(true, bitArray.GetValue(8, 8));
            Assert.AreEqual(true, bitArray.GetValue(8, 9));
            Assert.AreEqual(true, bitArray.GetValue(9, 9));
            Assert.AreEqual(true, bitArray.GetValue(9, 8));
        }
        
        [Test]
        public void InvertArea_InvalidXY_NotSetValues()
        {
            // Arrange
            var bitArray = new BitArray2();
            const int width = 10;
            const int height = 10;
            
            // Act
            bitArray.Init(width, height);
            var res1 = bitArray.InvertArea(9, 8, 2, 2);
            var res2 = bitArray.InvertArea(-1, 0, 2, 2);
            
            // Assert
            Assert.AreEqual(false, res1);
            Assert.AreEqual(false, res2);
            Assert.AreEqual(false, bitArray.GetValue(0, 0));
            Assert.AreEqual(false, bitArray.GetValue(0, 1));
            Assert.AreEqual(false, bitArray.GetValue(8, 8));
            Assert.AreEqual(false, bitArray.GetValue(8, 9));
            Assert.AreEqual(false, bitArray.GetValue(9, 9));
            Assert.AreEqual(false, bitArray.GetValue(9, 8));
        }
    }
}
