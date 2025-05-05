using Source.Utilities.Inventory;
using Source.Utilities.Inventory.Controllers;
using Source.Utilities.Inventory.Items;
using Source.Utilities.Inventory.Views;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Source.Components
{
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float velocity;

        [Header("Rotation")]
        [SerializeField] private float mouseSensitivity;
        [SerializeField, Range(-90.0f, 0.0f)] private float maxXRotation;
        [SerializeField, Range(0.0f, 90.0f)] private float minXRotation;

        [Header("Inventory")]
        [SerializeField] private UIDocument inventoryDocument;
        [SerializeField, Min(0.0f)] private int maxPickupDistance;
        [SerializeField, Min(0.0f)] private int maxDropDistance;
        [SerializeField, Min(1.0f)] private int inventoryWidth;
        [SerializeField, Min(1.0f)] private int inventoryHeight;
        [SerializeField] private float slotSize;
        [SerializeField] private float distanceBetweenSlots;
        
        [Header("Other")]
        [SerializeField] private UIDocument overlayDocument;
        
        private InventoryModel<IColoredItem, MouseInventoryController<IColoredItem>> _inventoryModel;
        private InputAction _moveHorizontalAction; 
        private InputAction _moveVerticalAction; 
        private InputAction _lookAction; 
        private InputAction _pickAction; 

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            var markup = new ColoredInventoryMarkup(
                inventoryWidth,
                inventoryHeight,
                slotSize,
                distanceBetweenSlots
            );
            var view = new ColoredInventoryView(markup);

            _inventoryModel = new InventoryModel<IColoredItem, MouseInventoryController<IColoredItem>>(
                inventoryDocument,
                view,
                inventoryWidth,
                inventoryHeight
            );
            
            _moveHorizontalAction = InputSystem.actions.FindAction("MoveHorizontal");
            _moveVerticalAction = InputSystem.actions.FindAction("MoveVertical");
            _lookAction = InputSystem.actions.FindAction("Look");
            _pickAction = InputSystem.actions.FindAction("Pickup");
        }

        private void Update()
        {
            if (Input.GetButtonDown("Inventory")) _inventoryModel.SwitchInventoryVisibility();
        }

        private void FixedUpdate()
        {
            if (_inventoryModel.IsInventoryVisible) return;
            ApplyRotation();
            ApplyMovement();
            if (_pickAction.IsPressed()) _inventoryModel.TryPickupItem(transform, maxPickupDistance, maxDropDistance);
        }

        private void ApplyRotation()
        {
            var delta = _lookAction.ReadValue<Vector2>();

            var xRotation = transform.localEulerAngles.x > 180
                ? transform.localEulerAngles.x - 360.0f
                : transform.localEulerAngles.x;
            var xDelta = Mathf.Clamp(-delta.y * mouseSensitivity, maxXRotation - xRotation, minXRotation - xRotation);

            transform.Rotate(new Vector3(0.0f, delta.x * mouseSensitivity, 0.0f), Space.World);
            transform.Rotate(xDelta, 0.0f, 0.0f, Space.Self);
        }

        private void ApplyMovement()
        {
            var horizontal = _moveHorizontalAction.ReadValue<Vector2>();
            var vertical = _moveVerticalAction.ReadValue<float>();

            var movementVector = new Vector3(horizontal.x, vertical, horizontal.y);
            if (movementVector.magnitude > 1.0f) movementVector.Normalize();

            transform.Translate(movementVector * velocity);
        }
    }
}