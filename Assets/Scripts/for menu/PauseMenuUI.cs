using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Instance;

    [SerializeField] private CanvasGroup pauseCanvasGroup;
    private UnifiedPlayerController playerController;
    
    [SerializeField] private GameObject gameOverObject;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        SetPauseCanvas(false);
    }

    private void Start()
    {
        playerController = FindObjectOfType<UnifiedPlayerController>();
    }

    public void TogglePause()
    {

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

        playerController?.ResumeWithInputDelay();
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene_1_MainMenu");
    }

    private void SetPauseCanvas(bool show)
    {
        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.alpha = show ? 1f : 0f;
            pauseCanvasGroup.interactable = show;
            pauseCanvasGroup.blocksRaycasts = show;

        }
    }
    
    public void ShowGameOver()
    {
        if (gameOverObject != null)
        {
            gameOverObject.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;

        if (playerController != null)
        {
            playerController.SetPaused(true);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameOverObject.activeSelf)
            {
                SceneManager.LoadScene("Scene_1_MainMenu");
            }
        }
    }


}