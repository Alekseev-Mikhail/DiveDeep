using Source.Utilities.Inventory.Items;
using Source.Utilities.Inventory.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace Source.Utilities.Inventory.Controllers
{
    public class MouseInventoryController<TItem> : PointerManipulator, IController<TItem> where TItem : IPlayerItem
    {
        private View<TItem> _view;
        private Inventory<TItem> _inventory;
        private TItem _item;
        
        private Vector2 _startTargetPosition;
        private Vector3 _startPointerPosition;
        private bool _isDragged;
        private int _pointerId = -1;
        
        public MouseInventoryController()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }

        public void Init(View<TItem> view, Inventory<TItem> inventory, TItem item)
        {
            _view = view;
            _inventory = inventory;
            _item = item;
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
            if (e.button == (int)MouseButton.RightMouse) DropItem(); 
            
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

            var nearestSlot = _view.GetNearestSlot(target.layout.x, target.layout.y);
            if (_inventory.MoveItem(_item, nearestSlot.x, nearestSlot.y)) SetNewPosition(nearestSlot);
            else SetStartPosition();

            _isDragged = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }

        private void DropItem()
        {
            if (!_inventory.RemoveItem(_item)) return;
            target.RemoveFromHierarchy();
            _item.Drop();
        }
        
        private void SetNewPosition(Vector2Int nearestSlot)
        {
            var position = _view.GetSlotPosition(nearestSlot.x, nearestSlot.y);
            target.style.left = position.x;
            target.style.top = position.y;
        }

        private void SetStartPosition()
        {
            target.style.left = _startTargetPosition.x;
            target.style.top = _startTargetPosition.y;
        }
    }
}