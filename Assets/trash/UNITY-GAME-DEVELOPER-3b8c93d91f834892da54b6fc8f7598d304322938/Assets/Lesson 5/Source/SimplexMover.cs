using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lesson5
{
    public class SimplexMover : MonoBehaviour
    {
        public const float GIZMOS_SPHERE_RADIUS = .125f;
        
        [SerializeField] private Transform _self;
        [SerializeField] private Vector3 _start;
        [SerializeField] private Vector3 _end;
        [SerializeField] private float _speed;
        [SerializeField] private MeshRenderer _selfRenderer;
        [SerializeField] private float _saturation;
        [SerializeField] private float _value;

        private Vector3 _position;
        
        public void Init()
        {
            Color color = Color.HSVToRGB(Random.value, _saturation, _value);
            _selfRenderer.material.SetColor("_BaseColor", color);
            _self.position = _position = _start;
        }

        private void OnEnable()
        {
            _self.position = _position = _start;
        }

        private void Update()
        {
            float interval = _speed * Time.deltaTime;
            float distance = (_end - _position).magnitude;

            if (distance <= interval)
            {
                enabled = false;
                return;
            }
            
            Quaternion rotation = Random.rotation;
            _position = Vector3.MoveTowards(_position, _end, interval);
            _self.SetPositionAndRotation(_position, rotation);
        }

        private void OnDisable()
        {
            _self.position = _end;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_start, GIZMOS_SPHERE_RADIUS);
            Gizmos.DrawSphere(_end, GIZMOS_SPHERE_RADIUS);
            Gizmos.DrawLine(_start, _end);
        }
    }
}