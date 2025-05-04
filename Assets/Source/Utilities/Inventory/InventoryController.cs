using Source.Utilities.Inventory.Items;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Source.Utilities.Inventory
{
    public class InventoryController<T> where T : IPlayerItem
    {
        private readonly Inventory<T> _inventory;
        private readonly IView<T> _view;
        private readonly UIDocument _document;
        public bool IsInventoryVisible { get; private set; }

        public InventoryController(UIDocument document, IView<T> view, int width, int height)
        {
            _inventory = new Inventory<T>(width, height);
            _view = view;
            _document = document;
            _document.enabled = false;
        }

        public void PutItem(T item, int x, int y)
        {
            _inventory.PutItem(item, x, y);
        }
        
        public void SwitchInventoryVisibility()
        {
            if (!IsInventoryVisible) ShowInventory();
            else HideInventory();
            IsInventoryVisible = !IsInventoryVisible;
        }

        private void ShowInventory()
        {
            _document.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            _view.ConstructView(_document, _inventory);
        }

        private void HideInventory()
        {
            _document.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            _view.BreakDown();
        }
    }
}
