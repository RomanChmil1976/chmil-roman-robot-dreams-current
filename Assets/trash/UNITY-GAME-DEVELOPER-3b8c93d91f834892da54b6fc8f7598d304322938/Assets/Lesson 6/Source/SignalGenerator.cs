namespace Lesson6
{
    public class SignalGenerator
    {
        private float _waveLength;
        private float _threshold;

        public SignalGenerator(float waveLength, float threshold)
        {
            _waveLength = waveLength;
            _threshold = threshold;
        }
        
        public float EvaluateSignal(float time)
        {
            float normalizedTime = (time * (1f / _waveLength)) % 1f;

            return normalizedTime <= _threshold ? 0f : 1f;
        }
    }
}