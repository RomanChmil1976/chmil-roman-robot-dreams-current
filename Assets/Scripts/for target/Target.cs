using UnityEngine;
using System;
using System.Collections;

public class Target : MonoBehaviour
{
    public float maxHealth = 100f;
    public float respawnTime = 3f;
    private float currentHealth;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Renderer[] renderers;
    private Collider[] colliders;

    [SerializeField] private HealthBar healthBar;

    //  Новый ивент — уведомляет, что цель уничтожена
    public event Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        UpdateHealthUI();

        //  Подписка на событие смерти (одноразовая, но можно через ScoreManager.RegisterTarget(this))
        if (ScoreManager.Instance != null)
        {
            OnDeath += () => ScoreManager.Instance.AddScore(1);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

    IEnumerator RespawnRoutine()
    {
        DisableTarget();

        //  Удалено: ScoreManager.Instance.AddScore(1);
        //  Вызов события смерти (если есть подписчики)
        OnDeath?.Invoke();

        yield return new WaitForSecondsRealtime(respawnTime);

        Respawn();
    }

    void DisableTarget()
    {
        foreach (var r in renderers) r.enabled = false;
        foreach (var c in colliders) c.enabled = false;
        if (healthBar != null) healthBar.gameObject.SetActive(false);
    }

    void Respawn()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentHealth = maxHealth;

        foreach (var r in renderers) r.enabled = true;
        foreach (var c in colliders) c.enabled = true;

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            UpdateHealthUI();
        }

        Debug.Log($"{gameObject.name} respawned!");
    }
}
