using System;
using UnityEngine;

namespace Lesson5
{
    public class LifecycleSample : IDisposable
    {
        private static int instanceId;

        private int _instanceId;

        //private string[] _garbage;
        
        public LifecycleSample()
        {
            _instanceId = instanceId++;
            Debug.Log($"Lifecycle Sample Constructor! '{_instanceId:x8}'");
            /*_garbage = new string[10000000];
            for (int i = 0; i < _garbage.Length; ++i)
            {
                _garbage[i] = "A lot of Garbage for collector to collect";
            }*/
        }

        public void Dispose()
        {
            // TODO release managed resources here
            Debug.Log($"Lifecycle Sample Disposed! '{_instanceId:x8}'");
        }

        ~LifecycleSample()
        {
            Debug.Log($"Lifecycle Sample Destructor! '{_instanceId:x8}'");
        }
    }
}