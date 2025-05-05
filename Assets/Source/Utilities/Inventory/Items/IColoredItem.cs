using UnityEngine;

namespace Source.Utilities.Inventory.Items
{
    public interface IColoredItem : IPlayerItem
    {
        public Color Color { get; }
    }
}