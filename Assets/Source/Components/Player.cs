using Source.Utilities;
using Source.Utilities.Inventory;
using Source.Utilities.Inventory.Items;
using Source.Utilities.Inventory.Views;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Source.Components
{
    public class Player : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float velocity;

        [Header("Rotation")] [SerializeField] private float mouseSensitivity;
        [SerializeField, Range(-90.0f, 0.0f)] private float maxXRotation;
        [SerializeField, Range(0.0f, 90.0f)] private float minXRotation;

        [Header("Inventory")] [SerializeField] private UIDocument inventoryDocument;
        [SerializeField, Min(1.0f)] private int inventoryWidth;
        [SerializeField, Min(1.0f)] private int inventoryHeight;
        [SerializeField] private float slotSize;
        [SerializeField] private float distanceBetweenSlots;

        private InventoryController<ColoredItem> _inventoryController;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            var preferences = new InventoryMarkup(
                inventoryWidth,
                inventoryHeight,
                slotSize,
                distanceBetweenSlots
            );
            var view = new ColoredInventoryView(preferences);

            _inventoryController = new InventoryController<ColoredItem>(
                inventoryDocument,
                view,
                inventoryWidth,
                inventoryHeight
            );

            _inventoryController.PutItem(new ColoredItem(1, 1, new Color(1, 0, 0)), 2, 0);
            _inventoryController.PutItem(new ColoredItem(2, 2, new Color(0, 1, 0)), 3, 3);
            _inventoryController.PutItem(new ColoredItem(1, 3, new Color(0, 0, 1)), 0, 4);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Inventory")) _inventoryController.SwitchInventoryVisibility();
        }

        private void FixedUpdate()
        {
            if (_inventoryController.IsInventoryVisible) return;
            ApplyRotation();
            ApplyMovement();
        }

        private void ApplyRotation()
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");

            var xRotation = transform.localEulerAngles.x > 180
                ? transform.localEulerAngles.x - 360.0f
                : transform.localEulerAngles.x;
            var xDelta = Mathf.Clamp(-mouseY * mouseSensitivity, maxXRotation - xRotation, minXRotation - xRotation);

            transform.Rotate(new Vector3(0.0f, mouseX * mouseSensitivity, 0.0f), Space.World);
            transform.Rotate(xDelta, 0.0f, 0.0f, Space.Self);
        }

        private void ApplyMovement()
        {
            var forward = Input.GetAxis("Forward");
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var movementVector = new Vector3(horizontal, vertical, forward);
            if (movementVector.magnitude > 1.0f) movementVector.Normalize();

            transform.Translate(movementVector * velocity);
        }
    }
}