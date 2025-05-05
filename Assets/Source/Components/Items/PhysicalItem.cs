using Source.Utilities.Inventory.Items;
using UnityEngine;

namespace Source.Components.Items
{
    public abstract class PhysicalItem : MonoBehaviour, IColoredItem
    {
        private Transform _player;
        private float _maxDropDistance;
        
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract Color Color { get; }
        
        public void Pickup(Transform player, float maxDropDistance)
        {
            _player = player;
            _maxDropDistance = maxDropDistance;
            gameObject.SetActive(false);
        }

        public void Drop()
        {
            gameObject.SetActive(true);
            transform.position = _player.forward * _maxDropDistance + _player.position;
        }
    }
}