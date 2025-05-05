using UnityEngine;

namespace Source.Utilities.Inventory.Items
{
    public interface IPlayerItem
    {
        public int Width { get; } 
        public int Height { get; }
        
        public void Pickup(Transform player, float maxDropDistance);
        
        public void Drop();
    }
}