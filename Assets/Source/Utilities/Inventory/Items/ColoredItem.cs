using UnityEngine;

namespace Source.Utilities.Inventory.Items
{
    public class ColoredItem : IPlayerItem
    {
        public int Width { get; }
        public int Height { get; }
        public Color Color { get; }
        
        public ColoredItem(int width, int height, Color color)
        {
            Width = width;
            Height = height;
            Color = color;
        }
    }
}