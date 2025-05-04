using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Components
{
    public class ChainBehaviour : Chain
    {
        [Header("Behaviour")]
        [SerializeField] private float velocity;
        [SerializeField, Range(0.0f, 1.0f)] private float maneuverability;
        [SerializeField, Min(0.0f)] private float targetBoxSize;
        [SerializeField, Min(0.0f)] private float triggerBoxSize;
        
        [Header("Sine Wave")]
        [SerializeField, Min(0.01f)] private float waveLength;
        [SerializeField, Min(0.0f)] private float minWaveAmplitude;
        [SerializeField, Min(1.0f)] private float maxWaveAmplitude;

        private Vector3 _target;

        protected override void LateInit()
        {
            UpdateTarget();
        }

        protected override void DoPreUpdate()
        {
            MoveForward();
        }

        private void UpdateTarget()
        {
            _target = new Vector3
            {
                x = Random.Range(-targetBoxSize, targetBoxSize),
                y = Random.Range(-targetBoxSize, targetBoxSize),
                z = Random.Range(-targetBoxSize, targetBoxSize)
            };
        }
        
        private void MoveForward()
        {
            var temp = head.transform.rotation;
            head.transform.LookAt(_target);
            var targetRotation = head.transform.rotation;
            head.transform.rotation = temp;

            var sinAngle = Mathf.Sin(Time.fixedTime / waveLength) * Random.Range(minWaveAmplitude, maxWaveAmplitude);
            
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, targetRotation, maneuverability);
            head.transform.Rotate(Vector3.up, sinAngle, Space.World);
            head.transform.Translate(0.0f, 0.0f, velocity, Space.Self);

            if (Vector3.Distance(head.transform.position, _target) < triggerBoxSize) UpdateTarget();
        }
    }
}
