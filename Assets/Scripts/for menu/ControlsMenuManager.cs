using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenuManager : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "Scene_1_MainMenu"; // Название сцены

    // Кнопка "Back" → возвращаемся в главное меню
    public void BackToMenu()
    {
        Debug.Log("🔙 Возвращаемся в главное меню...");
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}