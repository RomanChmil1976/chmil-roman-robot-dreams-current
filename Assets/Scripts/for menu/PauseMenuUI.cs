using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Instance;

    [SerializeField] private CanvasGroup pauseCanvasGroup;
    private UnifiedPlayerController playerController;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        SetPauseCanvas(false); // Скрываем паузу при старте
    }

    private void Start()
    {
        playerController = FindObjectOfType<UnifiedPlayerController>();
    }

    public void TogglePause()
    {
        Debug.Log("🟡 TogglePause вызван, текущее состояние isPaused = " + isPaused);

        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        SetPauseCanvas(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;

        playerController?.SetPaused(true);
    }

    public void ResumeGame()
    {
        SetPauseCanvas(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;

        playerController?.SetPaused(false);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        SceneManager.LoadScene("Scene_1_MainMenu");
#endif
    }

    private void SetPauseCanvas(bool show)
    {
        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.alpha = show ? 1f : 0f;
            pauseCanvasGroup.interactable = show;
            pauseCanvasGroup.blocksRaycasts = show;

            Debug.Log($"🔧 PauseCanvas Set: show = {show}, alpha = {pauseCanvasGroup.alpha}");
        }
    }
}