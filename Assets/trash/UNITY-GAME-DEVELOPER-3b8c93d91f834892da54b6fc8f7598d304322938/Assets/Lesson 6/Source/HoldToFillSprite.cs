using System;
using UnityEngine;

namespace Lesson6
{
    public class HoldToFillSprite : MonoBehaviour
    {
        [SerializeField] private float _startFillDelay;
        [SerializeField] private float _fillSpeed;

        [SerializeField] private Color _fillColor;
        [SerializeField] private Color _completedColor;

        [SerializeField] private SpriteRenderer _fillSpriteRenderer;
        [SerializeField] private float _height;
        [SerializeField] private Vector2 _fillBounds;

        private HoldTimer _timer;

        private void OnEnable()
        {
            _fillSpriteRenderer.color = _fillColor;
            _fillSpriteRenderer.size = new Vector2(_fillBounds.x, _height);
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
                _fillSpriteRenderer.color = _completedColor;
                _fillSpriteRenderer.size = new Vector2(_fillBounds.y, _height);
                return;
            }
            _fillSpriteRenderer.size = new Vector2(Mathf.Lerp(_fillBounds.x, _fillBounds.y, _timer.Progress), _height);
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
            _fillSpriteRenderer.size = new Vector2(_fillBounds.x, _height);
        }
    }
}