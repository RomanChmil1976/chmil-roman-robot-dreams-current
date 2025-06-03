using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Canvas winCanvas; 
    [SerializeField] private int winScore = 50; 

    private bool hasWon = false; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        TargetManager.onTargetSpawn += RegisterTarget;
        TargetManager.onTargetDespawn += UnregisterTarget;
    }

    private void OnDisable()
    {
        TargetManager.onTargetSpawn -= RegisterTarget;
        TargetManager.onTargetDespawn -= UnregisterTarget;
    }

    private void RegisterTarget(Target target)
    {
        target.OnDeath += OnTargetDied;
    }

    private void UnregisterTarget(Target target)
    {
        target.OnDeath -= OnTargetDied;
    }

    private void OnTargetDied()
    {
        AddScore(1);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();

        if (!hasWon && score >= winScore)
        {
            hasWon = true;
            ShowWinCanvas();
            DisableBots();
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void ShowWinCanvas()
    {
        if (winCanvas != null)
        {
            winCanvas.gameObject.SetActive(true);  // Включаем Canvas
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void DisableBots()
    {
        foreach (var bot in FindObjectsOfType<Target>())
        {
            if (!bot.CompareTag("Player"))
            {
                Destroy(bot.gameObject); 
            }
        }
    }
}