namespace Source.Utilities
{
    public class BitArray2
    {
        private int _width;
        private int _height;
        private int _columnSize;
        private byte[][] _slots;

        public BitArray2(int width, int height, byte[][] slots)
        {
            _width = width;
            _height = height;
            _slots = slots;
        }

        public BitArray2()
        {
        }

        public bool Init(int width, int height)
        {
            if (width <= 0 || height <= 0) return false;
            _width = width;
            _height = height;
            _slots = new byte[width][];
            _columnSize = height % sizeof(byte) != 0 ? height / sizeof(byte) + 1 : height / sizeof(byte);
            for (var i = 0; i < width; i++) _slots[i] = new byte[_columnSize];
            return true;
        }

        public BitArray2 Copy()
        {
            var slots = new byte[_width][];
            for (var x = 0; x < _width; x++)
            {
                slots[x] = new byte[_columnSize];
                for (var y = 0; y < _columnSize; y++)
                {
                    slots[x][y] = _slots[x][y];
                }
            }
            return new BitArray2(_width, _height, slots);
        }

        public int GetWidth() => _width;

        public int GetHeight() => _height;

        /// Retrieves the value of a specific bit in the array, identified by its x and y coordinates.
        /// <param name="x">
        /// The x-coordinate of the bit to retrieve.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the bit to retrieve.
        /// </param>
        /// <returns>
        /// Returns true if the specified bit is set; otherwise, false. If the coordinates are out of bounds,
        /// the method returns false.
        /// </returns>
        public bool GetValue(int x, int y)
        {
            if (x >= _width || x < 0 || y >= _height || y < 0) return false;
            var chunk = _slots[x][y / sizeof(byte)];
            var bitMask = (byte)(1 << (y % sizeof(byte)));
            return (chunk & bitMask) == 1;
        }

        /// Inverts the state of all bits within a specified rectangular region of the array.
        /// If the region exceeds the bounds of the array, no changes are made, and the method returns false.
        /// <param name="x">
        /// The x-coordinate of the top-left corner of the region to invert.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the top-left corner of the region to invert.
        /// </param>
        /// <param name="width">
        /// The width of the region to invert.
        /// </param>
        /// <param name="height">
        /// The height of the region to invert.
        /// </param>
        /// <returns>
        /// Returns true if the specified area was successfully inverted.
        /// </returns>
        public bool InvertArea(int x, int y, int width, int height)
        {
            if (x + width > _width || x < 0 || y + height > _height || y < 0) return false;
            var endX = x + width;
            var endY = y + height;
            for (var i = x; i < endX; i++)
            {
                for (var j = y; j < endY; j++)
                {
                    Invert(i, j);
                }
            }

            return true;
        }

        /// Inverts the value of a specific bit in the array at the given x and y coordinates.
        /// <param name="x">
        /// The x-coordinate of the bit to invert.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the bit to invert.
        /// </param>
        /// <returns>
        /// Returns true if the bit was successfully inverted. Returns false if the coordinates are out of bounds.
        /// </returns>
        public bool Invert(int x, int y)
        {
            if (x >= _width || x < 0 || y >= _height || y < 0) return false;
            var chunk = _slots[x][y / sizeof(byte)];
            var bitMask = (byte)(1 << (y % sizeof(byte)));
            _slots[x][y / sizeof(byte)] = (byte)(chunk ^ bitMask);
            return true;
        }
    }
}