using System;
using UnityEngine;

namespace Lesson5
{
    public class LifecycleController : MonoBehaviour
    {
        private LifecycleSample _sample;
        private LifecycleSample _hiddenSample;

        [ContextMenu("Create Sample")]
        private void CreateSample()
        {
            RemoveSample();
            _sample = new LifecycleSample();
        }

        [ContextMenu("Hide Sample")]
        private void HideSample()
        {
            _hiddenSample = _sample;
        }
        
        [ContextMenu("UnHide Sample")]
        private void UnHideSample()
        {
            _hiddenSample = null;
        }
        
        [ContextMenu("Remove Sample")]
        private void RemoveSample()
        {
            if (_sample != null)
            {
                _sample.Dispose();
            }

            //
            _sample = null;
        }

        [ContextMenu("Collect Garbage")]
        private void CollectGarbage()
        {
            GC.Collect();
        }
    }
}