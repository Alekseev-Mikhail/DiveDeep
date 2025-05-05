using Source.Utilities.Inventory.Items;
using Source.Utilities.Inventory.Views;
using UnityEngine.UIElements;

namespace Source.Utilities.Inventory.Controllers
{
    public interface IController<TItem> : IManipulator where TItem : IPlayerItem
    {
        public void Init(View<TItem> view, Inventory<TItem> inventory, TItem item);
    }
}