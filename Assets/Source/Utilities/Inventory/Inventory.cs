using System;
using System.Collections.Generic;
using Source.Utilities.Inventory.Items;
using UnityEngine;

namespace Source.Utilities.Inventory
{
    public class Inventory<T> where T : IPlayerItem
    {
        private readonly Dictionary<T, Vector2Int> _items = new();
        private readonly BitArray2 _bitArray = new();

        public ImmutableDictionary<T, Vector2Int> GetItems() => _items.ToImmutable();

        public Inventory(int width, int height)
        {
            if (!_bitArray.Init(width, height))
                throw new ArgumentException("Invalid inventory size. Width and height must be greater than 0.");
        }

        public int GetWidth() => _bitArray.GetWidth();

        public int GetHeight() => _bitArray.GetHeight();

        public bool PutItem(T item, int x, int y)
        {
            if (_items.ContainsKey(item)) return false;
            var w = x + item.Width;
            var h = y + item.Height;
            if (w > _bitArray.GetWidth() || x < 0 || h > _bitArray.GetHeight() || y < 0) return false;
            if (!IsSpaceFree(item, x, y)) return false;

            _bitArray.InvertArea(x, y, item.Width, item.Height);
            _items.Add(item, new Vector2Int(x, y));
            return true;
        }

        public bool MoveItem(T item, int x, int y)
        {
            var currentPosition = _items[item];
            if (!IsSpaceFreeOrSame(item, currentPosition, x, y)) return false;
            if (!_bitArray.InvertArea(x, y, item.Width, item.Height)) return false;
            
            _bitArray.InvertArea(currentPosition.x, currentPosition.y, item.Width, item.Height);
            _items[item] = new Vector2Int(x, y);
            return true;
        }

        private bool IsSpaceFree(IPlayerItem item, int x, int y)
        {
            for (var i = x; i < x + item.Width; i++)
            {
                for (var j = y; j < y + item.Height; j++)
                {
                    if (_bitArray.GetValue(i, j)) return false;
                }
            }

            return true;
        }
        
        private bool IsSpaceFreeOrSame(IPlayerItem item, Vector2Int currentPosition, int x, int y)
        {
            for (var i = x; i < x + item.Width; i++)
            {
                for (var j = y; j < y + item.Height; j++)
                {
                    var inRangeX = i >= currentPosition.x && i < currentPosition.x + item.Width;
                    var inRangeY = j >= currentPosition.y && j < currentPosition.y + item.Height;
                    if (inRangeX && inRangeY) continue;
                    if (_bitArray.GetValue(i, j)) return false;
                }
            }

            return true;
        }
    }
}