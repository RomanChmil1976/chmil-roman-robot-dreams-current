using System;
using UnityEngine;

namespace Lesson5
{
    [Serializable]
    public class SerializationData
    {
        [SerializeField] private int _intValue;
        [SerializeField] private float _floatValue;
        [SerializeField] private bool _boolValue;
        [SerializeField] private string _textValue;
        [SerializeField] private SampleEnum _enumValue;
        [SerializeField] private Vector3 _positionValue;
        [SerializeField] private Quaternion _rotationValue;
        [SerializeField] private int[] _intArray;
        [SerializeField] private float[] _floatArray;
        [SerializeField] private bool[] _boolArray;
        [SerializeField] private string[] _textArray;
        [SerializeField] private SampleEnum[] _enumArray;
    }
}