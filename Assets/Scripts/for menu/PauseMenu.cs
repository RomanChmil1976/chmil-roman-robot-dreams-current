using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "Scene_1_MainMenu";
    private const string GAME_SCENE = "Scene_4_Game_1";

    private void Start()
    {
        // Когда открывается меню паузы — разблокируем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("⏸ Пауза активирована — курсор разблокирован");
    }

    // Кнопка RESUME
    public void ResumeGame()
    {
        // Закрываем сцену паузы
        SceneManager.UnloadSceneAsync("Scene_1.2_Pause");

        // Возвращаем курсор в игровой режим
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("▶ Продолжение игры — курсор скрыт и заблокирован");
    }

    // Кнопка EXIT
    public void ExitGame()
    {
        Debug.Log("🚪 Выход из игры...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // для редактора
#else
        Application.Quit(); // для билдов
#endif
    }
}