using Source.Utilities.Inventory;
using Source.Utilities.Inventory.Items;
using UnityEngine.UIElements;

namespace Source.Utilities
{
    public interface IView<T> where T : IPlayerItem
    {
        public void ConstructView(UIDocument document, Inventory<T> inventory);

        public void BreakDown();
    }
}