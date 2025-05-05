using Source.Utilities.Inventory.Items;
using UnityEngine;

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

        public void Pickup(Transform player, float maxDropDistance)
        {
        }

        public void Drop()
        {
        }
    }
}