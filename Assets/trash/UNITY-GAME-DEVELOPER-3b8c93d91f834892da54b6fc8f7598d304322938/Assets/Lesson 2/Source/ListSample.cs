using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSample : MonoBehaviour
{
    [SerializeField] private string _value;
    
    [SerializeField] private List<string> _list;

    [ContextMenu("Print")]
    private void Print()
    {
        string msg = "List: ";
        for (int i = 0; i < _list.Count; ++i)
            msg += $"\n{_list[i]}";
        Debug.Log(msg);
    }
}
