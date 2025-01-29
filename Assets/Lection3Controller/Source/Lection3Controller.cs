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
            if (list.Count == 0)
            {
                Debug.Log("List is empty.");
            }
            else
            {
                string msg = "List:\n" + string.Join("\n", list);
                Debug.Log(msg);
            }
        }

        [ContextMenu("Clear")]
        private void ClearList()
        {
            list.Clear();
            Debug.Log("Cleared.");
        }
    }
}