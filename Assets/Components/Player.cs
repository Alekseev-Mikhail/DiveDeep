using UnityEngine;

namespace Components
{
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float velocity;
        
        [Header("Rotation")]
        [SerializeField] private float mouseSensitivity;
        [SerializeField, Range(-90.0f, 0.0f)] private float maxXRotation;
        [SerializeField, Range(0.0f, 90.0f)] private float minXRotation;

        private void FixedUpdate()
        {
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