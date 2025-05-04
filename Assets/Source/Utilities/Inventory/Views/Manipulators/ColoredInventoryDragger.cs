using Source.Utilities.Inventory.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace Source.Utilities.Inventory.Views.Manipulators
{
    public class ColoredInventoryDragger : PointerManipulator
    {
        private readonly ColoredItem _item;
        private readonly Inventory<ColoredItem> _inventory;
        private readonly InventoryMarkup _markup;

        private Vector2 _startTargetPosition;
        private Vector3 _startPointerPosition;
        private bool _isDragged;
        private int _pointerId = -1;

        public ColoredInventoryDragger(ColoredItem item, Inventory<ColoredItem> inventory, InventoryMarkup markup)
        {
            _item = item;
            _inventory = inventory;
            _markup = markup;
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private void OnPointerDown(PointerDownEvent e)
        {
            if (_isDragged)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (!CanStartManipulation(e)) return;

            _startPointerPosition = e.localPosition;
            _startTargetPosition = new Vector2(target.layout.x, target.layout.y);
            _pointerId = e.pointerId;
            _isDragged = true;

            target.CapturePointer(_pointerId);
            e.StopPropagation();

            target.BringToFront();
        }

        private void OnPointerMove(PointerMoveEvent e)
        {
            if (!_isDragged || !target.HasPointerCapture(_pointerId)) return;

            Vector2 difference = e.localPosition - _startPointerPosition;

            target.style.top = target.layout.y + difference.y;
            target.style.left = target.layout.x + difference.x;

            e.StopPropagation();
        }

        private void OnPointerUp(PointerUpEvent e)
        {
            if (!_isDragged || !target.HasPointerCapture(_pointerId) || !CanStopManipulation(e)) return;

            var x = Mathf.RoundToInt((target.layout.x - _markup.Margin) / _markup.SizeAndMargin);
            var y = Mathf.RoundToInt((target.layout.y - _markup.Margin) / _markup.SizeAndMargin);
            if (_inventory.MoveItem(_item, x, y))
            {
                target.style.top = _markup.SizeAndMargin * y + _markup.Margin;
                target.style.left = _markup.SizeAndMargin * x + _markup.Margin;
            }
            else
            {
                target.style.top = _startTargetPosition.y;
                target.style.left = _startTargetPosition.x;
            }

            _isDragged = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }
    }
}