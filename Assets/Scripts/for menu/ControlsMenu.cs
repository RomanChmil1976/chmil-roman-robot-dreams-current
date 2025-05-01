using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "Scene_1_MainMenu";
    public void BackToMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}