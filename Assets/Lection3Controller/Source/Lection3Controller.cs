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
            if (list.Contains(value))
            {
                list.Remove(value);
                Debug.Log($"Removed: {value}");
            }
            else
            {
                Debug.Log($"No instance of {value} to remove");
            }
        }

        [ContextMenu("RemoveDuplicates")]
        private void RemoveDuplicates()
        {
            List<int> indicesForRemove = new List<int>();
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i] == value)
                {
                    indicesForRemove.Add(i);
                }
            }

            if (indicesForRemove.Count > 0)
            {
                for (int i = indicesForRemove.Count - 1; i >= 0; i--)
                {
                    int indexToRemove = indicesForRemove[i];
                    list.RemoveAt(indexToRemove);
                }

                Debug.Log($"Removed all instances of: {value}");
            }
            else
            {
                Debug.Log($"No instances to remove");
            }
        }

        [ContextMenu("Clear")]
            private void Clear()
            {
                list.Clear();
                Debug.Log("Cleared");
            }
        
        [ContextMenu("Sort")]
        private void Sort()
        {
            list.Sort();
            Debug.Log("Sorted");
        }
    }
}