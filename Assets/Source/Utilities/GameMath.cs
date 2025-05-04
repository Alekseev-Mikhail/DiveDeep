using UnityEngine;

namespace Source.Utilities
{
    public static class GameMath
    {
        public const float PI90 = Mathf.PI / 2.0f;
        public const float PI180 = Mathf.PI;
        public const float PI360 = Mathf.PI * 2.0f;
        
        public static float ClampAngle(float angle, float left, float right, float center)
        {
            var angleAndCircle = angle + PI360;

            if (angle < left && angle > right || angleAndCircle < left && angleAndCircle > right) return angle;

            var distance = angle - center;
            var absDistance = Mathf.Abs(distance);
            float first;
            float second;

            if (distance < 0)
            {
                first = left;
                second = right;
            }
            else
            {
                first = right;
                second = left;
            }

            return absDistance < PI360 - absDistance ? second : first;
        }
        
        public static Vector2 GetAngles(Vector3 basePoint, Vector3 toPoint)
        {
            var delta = toPoint - basePoint;
            
            var angles = new Vector2(
                delta.y != 0.0f ? Mathf.Atan(delta.z / delta.y) : 0.0f,
                delta.z != 0.0f ? Mathf.Atan(delta.x / delta.z) : 0.0f
            );
            
            // if (delta.y < 0.0f) angles.x += PI180;
            // else if (delta.z < 0.0f) angles.x += PI360;
            
            if (delta.y < 0.0f) angles.x += PI180;
            else
            {
                var isZero = delta.y == 0.0f; 
                if (isZero) angles.x += PI90;
                if (delta.z < 0.0f)
                {
                    if (isZero) angles.x += PI180;
                    else angles.x += PI360;
                }
            }
            
            if (delta.z < 0.0f) angles.y += PI180;
            else
            {
                var isZero = delta.z == 0.0f; 
                if (isZero) angles.y += PI90;
                if (delta.x < 0.0f)
                {
                    if (isZero) angles.y += PI180;
                    else angles.y += PI360;
                }
            }
            
            if (angles.x >= PI360) angles.x -= PI360;
            if (angles.y >= PI360) angles.y -= PI360;

            return angles;
        }
    }
}