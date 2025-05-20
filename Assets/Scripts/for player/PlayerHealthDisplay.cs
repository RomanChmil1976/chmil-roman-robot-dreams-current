using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Target playerTarget;

    private void Start()
    {
        if (playerTarget != null)
        {
            playerTarget.onHealthChanged += UpdateHealthText;
            playerTarget.onSpawn += HandleSpawn;
        }
    }

    private void HandleSpawn()
    {
        // 
        UpdateHealthText(playerTarget.CurrentHealth);
    }

    private void UpdateHealthText(float hp)
    {
        if (healthText != null)
            healthText.text = "Health: " + Mathf.Max(0, Mathf.RoundToInt(hp));
    }
}