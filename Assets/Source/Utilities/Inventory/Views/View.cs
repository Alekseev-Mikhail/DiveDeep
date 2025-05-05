using System.Collections.Generic;
using Source.Utilities.Inventory.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace Source.Utilities.Inventory.Views
{
    public abstract class View<TItem> where TItem : IPlayerItem
    {
        public abstract Dictionary<TItem, VisualElement> ConstructView(UIDocument document, Inventory<TItem> inventory);
        
        public abstract void DeconstructView();
        
        public abstract Vector2Int GetNearestSlot(float x, float y);
        
        public abstract Vector2 GetSlotPosition(int x, int y);
    }
}