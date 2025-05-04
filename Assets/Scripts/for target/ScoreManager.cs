using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score;
    
    [SerializeField] private TextMeshProUGUI scoreText;


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
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}