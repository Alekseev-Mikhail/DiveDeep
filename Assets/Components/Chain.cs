using System.Collections.Generic;
using UnityEngine;
using static Components.GameMath;

namespace Components
{
    public abstract class Chain : MonoBehaviour
    {
        [Header("Basic Chain Settings")]
        [SerializeField] protected GameObject head;
        [SerializeField, Min(1.0f)] private int nodeNumber;
        [SerializeField, Min(0.0f)] private float distanceBetweenNodes;
        [SerializeField, Range(0.0f, 360.0f)] private float bendAngle;

        private readonly List<GameObject> _chainNodes = new();
        
        private float _maxLeftRad;
        private float _maxRightRad;

        private void Start()
        {
            var halfOfBendAngle = bendAngle / 2 * Mathf.Deg2Rad;
            _maxLeftRad = PI180 + halfOfBendAngle;
            _maxRightRad = PI180 - halfOfBendAngle;

            _chainNodes.Add(head);
            for (var i = 1; i < nodeNumber; i++)
            {
                var node = Instantiate(head, transform);
                node.name = "Node " + i;
                _chainNodes.Add(node);
            }

            LateInit();
        }
        
        private void FixedUpdate()
        {
            DoPreUpdate();
            UpdateChain();
        }

        protected abstract void LateInit();
        
        protected abstract void DoPreUpdate();

        private void UpdateChain()
        {
            var last = head;
            for (var i = 1; i < _chainNodes.Count; i++)
            {
                var current = _chainNodes[i];

                var angles = GetConstrainedAngles(last, current);
                current.transform.position = GetPositionByAngles(last.transform.position, angles);
                SetDirection(last, current);

                last = current;
            }
        }

        private Vector2 GetConstrainedAngles(GameObject last, GameObject current)
        {
            var angles = GetAngles(last.transform.position, current.transform.position);

            // var leftX = _maxLeftRad + last.transform.eulerAngles.x * Mathf.Deg2Rad + PI90;
            // var rightX = _maxRightRad + last.transform.eulerAngles.x * Mathf.Deg2Rad + PI90;
            // var centerX = (last.transform.eulerAngles.x - 180.0f) * Mathf.Deg2Rad + PI90;
            // centerX = centerX < 0 ? centerX + PI360 : centerX;

            var leftY = _maxLeftRad + last.transform.eulerAngles.y * Mathf.Deg2Rad;
            var rightY = _maxRightRad + last.transform.eulerAngles.y * Mathf.Deg2Rad;
            var centerY = (last.transform.eulerAngles.y - 180.0f) * Mathf.Deg2Rad;
            centerY = centerY < 0 ? centerY + PI360 : centerY;

            var clampedAngles = new Vector2
            {
                x = angles.x,//ClampAngle(angles.x, leftX, rightX, centerX),
                y = ClampAngle(angles.y, leftY, rightY, centerY)
            };

            return clampedAngles;
        }

        private Vector3 GetPositionByAngles(Vector3 basePoint, Vector2 angles)
        {
            var x = Mathf.Sin(angles.y) * distanceBetweenNodes;
            var z = Mathf.Cos(angles.y) * distanceBetweenNodes;
            var y = z / Mathf.Tan(angles.x);
            var delta = new Vector3(x, float.IsInfinity(y) ? 0.0f : y, z);
            delta.Normalize();
            delta *= distanceBetweenNodes;
            return basePoint + delta;
        }

        private static void SetDirection(GameObject last, GameObject current)
        {
            current.transform.LookAt(last.transform);
        }
    }
}