using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private const string PAUSE_SCENE = "Scene_1.2_Pause";

    private void Start()
    {
        GameStateManager.Instance.isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("⏸ Пауза активирована");
    }

    public void ResumeGame()
    {
        GameStateManager.Instance.isPaused = false;
        SceneManager.UnloadSceneAsync(PAUSE_SCENE);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("▶ Возврат к игре");
    }

    public void ExitGame()
    {
        Debug.Log("🚪 Выход из игры...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}