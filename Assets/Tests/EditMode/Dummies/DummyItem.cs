using Source.Utilities;
using Source.Utilities.Inventory.Items;

namespace Tests.EditMode.Dummies
{
    public class DummyItem : IPlayerItem
    {
        public DummyItem(byte width, byte height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }
    }
}