using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Названия сцен (избегаем опечаток)
    private const string SETTINGS_SCENE = "Scene_2_SettingsMenu";
    private const string CONTROLS_SCENE = "Scene_3_ControlsMenu";
    private const string GAME_SCENE = "Scene_4_Game_1";

    // Запуск игры
    public void StartGame()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.isGameAlreadyStarted)
        {
            // Просто скрываем меню (если оно было загружено additively)
            SceneManager.UnloadSceneAsync("Scene_1_MainMenu");
        }
        else
        {
            if (GameStateManager.Instance != null)
                GameStateManager.Instance.isGameAlreadyStarted = true;

            // Полная загрузка сцены игры
            SceneManager.LoadScene(GAME_SCENE);
        }
    }


// Открыть меню настроек
    public void OpenSettings()
    {
        SceneManager.LoadScene(SETTINGS_SCENE);
    }
    
    // Открыть меню управления
    public void OpenControls()
    {
        SceneManager.LoadScene(CONTROLS_SCENE);
    }

    // Выход из игры
    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Для редактора Unity
#else
            Application.Quit(); // Для сборки игры
#endif
    }
}