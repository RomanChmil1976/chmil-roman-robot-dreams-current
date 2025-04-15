// ✅ EventSystemSingleton.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemSingleton : MonoBehaviour
{
    private void Awake()
    {
        EventSystem[] systems = FindObjectsOfType<EventSystem>();
        if (systems.Length > 1)
        {
            Debug.LogWarning("🛑 Лишний EventSystem удалён!");
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}