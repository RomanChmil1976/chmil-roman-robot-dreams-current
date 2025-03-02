using System;
using UnityEngine;

namespace Lesson6
{
    public class HoldToFill : MonoBehaviour
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField] private float _startFillDelay;
        [SerializeField] private float _fillSpeed;

        [SerializeField] private Color _fillColor;
        [SerializeField] private Color _completedColor;

        [SerializeField] private Transform _fillTransform;
        [SerializeField] private Renderer _fillRenderer;
        [SerializeField] private Vector3 _scale;
        [SerializeField] private Vector2 _fillBounds;

        private HoldTimer _timer;

        private void OnEnable()
        {
            _fillRenderer.material.SetColor(BaseColor, _fillColor);
            _fillTransform.localScale = new Vector3(_scale.x, _scale.y, _fillBounds.x);
        }

        private void Update()
        {
            if (_timer == null)
            {
                return;
            }
            _timer.Update(Time.deltaTime);
            if (_timer.Progress >= 1f)
            {
                _timer = null;
                enabled = false;
                _fillRenderer.material.SetColor(BaseColor, _completedColor);
                _fillTransform.localScale = new Vector3(_scale.x, _scale.y, _fillBounds.y);
                return;
            }
            _fillTransform.localScale = new Vector3(_scale.x, _scale.y, Mathf.Lerp(_fillBounds.x, _fillBounds.y, _timer.Progress));
        }

        private void OnMouseDown()
        {
            _timer = new HoldTimer(_startFillDelay, _fillSpeed);
        }

        private void OnMouseUp()
        {
            if (!enabled)
            {
                return;
            }
            
            _timer = null;
            _fillTransform.localScale = new Vector3(_scale.x, _scale.y, _fillBounds.x);
        }
    }
}