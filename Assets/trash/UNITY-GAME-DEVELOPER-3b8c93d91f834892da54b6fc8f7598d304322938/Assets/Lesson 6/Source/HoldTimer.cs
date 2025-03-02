namespace Lesson6
{
    public class HoldTimer
    {
        private float _startFillDelay;
        private float _fillSpeed;
        
        private float _holdTime;
        private float _filledAmount;

        public float Progress => _filledAmount;

        public HoldTimer(float startFillDelay, float fillSpeed)
        {
            _startFillDelay = startFillDelay;
            _fillSpeed = fillSpeed;
        }

        public void Update(float deltaTime)
        {
            if (_holdTime < _startFillDelay)
            {
                _holdTime += deltaTime;
            }
            else
            {
                _filledAmount += _fillSpeed * deltaTime;
            }
        }
    }
}