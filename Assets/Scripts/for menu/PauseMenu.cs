using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        // Загружаем меню поверх, а не заменой
        SceneManager.LoadScene("Scene_1_MainMenu", LoadSceneMode.Additive);
    }
}
