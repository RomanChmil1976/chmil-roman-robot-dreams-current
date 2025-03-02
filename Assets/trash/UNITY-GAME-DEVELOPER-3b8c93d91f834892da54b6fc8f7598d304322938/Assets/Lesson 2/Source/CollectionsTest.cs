using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionsTest : MonoBehaviour
{
    [SerializeField] private int[] _integerArray;
    [SerializeField] private List<float> _floatList;

    [SerializeField] private Transform _curent;
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;

    [ContextMenu("Animation")]
    private void Animation()
    {
        _ = StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        //yield return null;
        float interval = _speed * Time.deltaTime;
        float distance = (_curent.position - _target.position).magnitude;
        while (distance > interval)
        {
            _curent.position = Vector3.MoveTowards(_curent.position, _target.position, interval);
            yield return null;
            distance = (_curent.position - _target.position).magnitude;
            interval = _speed * Time.deltaTime;
        }
        _curent.position = _target.position;
    }
    
    [ContextMenu("Average")]
    private void Average()
    {
        int sum = 0;
        for (int i = 0; i < _integerArray.Length; ++i)
        {
            sum += _integerArray[i];
        }

        int average = sum / _integerArray.Length;
        Debug.Log($"Average for array: {average}");

        float listSum = 0;

        foreach (float value in _floatList)
        {
            listSum += value;
        }
        
        /*for (int i = 0; i < _integerList.Count; ++i)
        {
            sum += _integerList[i];
        }*/
        float listAverage = listSum / _floatList.Count;
        Debug.Log($"Average for list: {listAverage}");
    }
}
