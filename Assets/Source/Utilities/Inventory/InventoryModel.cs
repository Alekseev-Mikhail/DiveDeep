using System.Collections.Generic;
using Source.Utilities.Inventory.Controllers;
using Source.Utilities.Inventory.Items;
using Source.Utilities.Inventory.Views;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Source.Utilities.Inventory
{
    public class InventoryModel<TItem, TController> where TItem : IPlayerItem where TController : IController<TItem>, new()
    {
        private readonly Inventory<TItem> _inventory;
        private readonly View<TItem> _view;
        private readonly List<IController<TItem>> _controllers = new();
        private readonly UIDocument _document;
        public bool IsInventoryVisible { get; private set; }

        public InventoryModel(
            UIDocument document,
            View<TItem> view,
            int width,
            int height
        )
        {
            _inventory = new Inventory<TItem>(width, height);
            _view = view;
            _document = document;
            _document.enabled = false;
        }

        public void SwitchInventoryVisibility()
        {
            if (!IsInventoryVisible) ShowInventory();
            else HideInventory();
            IsInventoryVisible = !IsInventoryVisible;
        }

        public void TryPickupItem(Transform origin, float maxDistance, float maxItemDropDistance)
        {
            Physics.Raycast(origin.position, origin.forward, out var info, maxDistance);
            if (!info.transform) return;
            if (!info.transform.TryGetComponent(out TItem item)) return;
            _inventory.PutItem(item);
            item.Pickup(origin, maxItemDropDistance);
        }

        private void ShowInventory()
        {
            _document.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            var itemElements = _view.ConstructView(_document, _inventory);
            foreach (var (item, element) in itemElements)
            {
                var controller = new TController();
                controller.Init(_view, _inventory, item);
                element.AddManipulator(controller);
                _controllers.Add(controller);
            }
        }

        private void HideInventory()
        {
            _document.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            _view.DeconstructView();
            foreach (var controller in _controllers)
            {
                controller.target.RemoveManipulator(controller);
            }
            _controllers.Clear();
        }
    }
}