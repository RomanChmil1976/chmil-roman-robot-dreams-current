using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lesson6
{
    public class HoldToFillCanvas : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _startFillDelay;
        [SerializeField] private float _fillSpeed;

        [SerializeField] private Color _fillColor;
        [SerializeField] private Color _completedColor;

        [SerializeField] private Image _fillImage;
        [SerializeField] private RectTransform _fillTransform;
        [SerializeField] private float _height;
        [SerializeField] private Vector2 _fillBounds;

        private HoldTimer _timer;

        private void OnEnable()
        {
            _fillImage.color = _fillColor;
            _fillTransform.sizeDelta = new Vector2(_fillBounds.x, _height);
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
                _fillImage.color = _completedColor;
                _fillTransform.sizeDelta = new Vector2(_fillBounds.y, _height);
                return;
            }
            _fillTransform.sizeDelta = new Vector2(Mathf.Lerp(_fillBounds.x, _fillBounds.y, _timer.Progress), _height);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _timer = new HoldTimer(_startFillDelay, _fillSpeed);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!enabled)
            {
                return;
            }
            
            _timer = null;
            _fillTransform.sizeDelta = new Vector2(_fillBounds.x, _height);
        }
    }
}