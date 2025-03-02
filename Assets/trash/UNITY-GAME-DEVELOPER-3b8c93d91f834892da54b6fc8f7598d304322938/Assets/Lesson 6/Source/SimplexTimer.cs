using System;

namespace Lesson6
{
    public class SimplexTimer : IDisposable
    {
        public event Action OnCompleted;
        
        private float _duration;
        private float _time;
        private float _reciprocal;

        public float Progress => _time * _reciprocal;
        public float Time => _time;
        
        public SimplexTimer(float duration)
        {
            _duration = duration;
            _reciprocal = 1f / duration;
            _time = 0f;
        }

        public void Update(float deltaTime)
        {
            _time += deltaTime;
            if (_time >= _duration)
            {
                _time = _duration;
                OnCompleted?.Invoke();
            }
        }

        public void Dispose()
        {
            OnCompleted = null;
        }
    }
}