using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private const string SETTINGS_SCENE = "Scene_2_SettingsMenu";
    private const string CONTROLS_SCENE = "Scene_3_ControlsMenu";
    private const string GAME_SCENE = "Scene_4_Game_1";

    public void StartGame()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(SETTINGS_SCENE);
    }

    public void OpenControls()
    {
        SceneManager.LoadScene(CONTROLS_SCENE);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}