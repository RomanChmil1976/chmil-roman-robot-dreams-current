using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

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

    private void RegisterTarget(Target target)
    {
        target._onDeathScoreCallback = () => OnTargetDied(target);
        target.OnDeath += target._onDeathScoreCallback;
    }

    private void UnregisterTarget(Target target)
    {
        if (target._onDeathScoreCallback != null)
            target.OnDeath -= target._onDeathScoreCallback;
    }

    private void OnTargetDied(Target target)
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