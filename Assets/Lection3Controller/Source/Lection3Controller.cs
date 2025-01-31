using System.Collections.Generic;
using UnityEngine;

namespace Lection3Controller.Source
{
    public class Lection3Controller : MonoBehaviour
    {
        [SerializeField] private string value;
        [SerializeField] private List<string> list;

        [ContextMenu("Print")]
        private void Print()
        {
            string msg = "List:";
            for (int i = 0; i < list.Count; ++i)
                msg += $"\n{list[i]}";
            Debug.Log(msg);
        }

        [ContextMenu("Add")]
        private void Add()
        {
            list.Add(value);
            Debug.Log($"Added: {value}");
        }

        [ContextMenu("Remove")]
        private void Remove()
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == value)
                {
                    list.RemoveAt(i);
                    Debug.Log($"Removed: {value} at position {i}");
                }
            } 
        }

        [ContextMenu("Clear")]
        private void Clear()
        {
            list.Clear();
            Debug.Log("Cleared.");
        }
        
        [ContextMenu("Sort")]
        private void Sort()
        {
            list.Sort();
            Debug.Log("Sorted.");
        }
    }
}