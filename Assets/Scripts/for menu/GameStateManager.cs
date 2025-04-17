using UnityEngine;



public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    public bool isPaused = false; // ⏸ Новый флаг для отслеживания паузы

    public bool isGameAlreadyStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // чтобы не было дубликатов при возврате
        }
    }
}