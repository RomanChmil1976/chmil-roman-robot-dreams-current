using System;
using UnityEngine;

namespace Lesson6
{
    public class SignalDebug : MonoBehaviour
    {
        [SerializeField] private float _waveLength;
        [SerializeField] private float _threshold;

        [SerializeField] private float _interval;
        [SerializeField] private int _sampleCount;
        
        private SignalGenerator _signalGenerator;

        private Vector3[] _points = Array.Empty<Vector3>();
        
        [ContextMenu("Update Signal")]
        private void UpdateSignal()
        {
            _signalGenerator = new SignalGenerator(_waveLength, _threshold);
            _points = new Vector3[_sampleCount];

            for (int i = 0; i < _sampleCount; ++i)
            {
                _points[i] = new Vector3(i * _interval, _signalGenerator.EvaluateSignal(i * _interval), 0f);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLineStrip(_points, false);
        }
    }
}