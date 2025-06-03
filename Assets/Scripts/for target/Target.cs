using UnityEngine;
using System;
using System.Collections;

public class Target : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Renderer[] renderers;
    private Collider[] colliders;

    public event Action onSpawn;
    public event Action<float> onHealthChanged;
    public event Action OnDeath;

    public float CurrentHealth => currentHealth;

    public PlayerDamageOverlay damageOverlay;

    public bool IsAlive { get; private set; } = true;

    [SerializeField] private Transform targetBodyTransform;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        currentHealth = maxHealth;
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        TargetManager.Instance?.Register(this);
        onSpawn?.Invoke();
    }

    private void OnDisable()
    {
        TargetManager.Instance?.Unregister(this);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        onHealthChanged?.Invoke(currentHealth);

        if (CompareTag("Player") && damageOverlay != null)
        {
            damageOverlay.ShowDamageOverlay();
        }

        if (currentHealth <= 0f)
        {
            if (CompareTag("Player"))
            {
                Die();
            }
            else
            {
                StartCoroutine(BotDie());
            }
        }
    }

    private void Update()
    {
        Animator anim = GetComponentInChildren<Animator>();
    }

    private void Die()
    {
        IsAlive = false;

        var playerController = GetComponent<UnifiedPlayerController>();
        if (playerController != null)
        {
            playerController.ExitAimMode();
            PauseMenuUI.Instance?.ShowGameOver();
        }

        if (targetBodyTransform != null)
        {
            Animator anim = targetBodyTransform.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("DieTrigger1");
            }
        }
    }

    private IEnumerator BotDie()
    {
        IsAlive = false;

        if (targetBodyTransform != null)
        {
            Animator anim = targetBodyTransform.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("DieTrigger2");
            }
        }

        yield return new WaitForSeconds(0.4f); 

        foreach (var r in renderers) r.enabled = false;
        foreach (var c in colliders) c.enabled = false;

        OnDeath?.Invoke();

        yield return new WaitForSecondsRealtime(3f);

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentHealth = maxHealth;

        foreach (var r in renderers) r.enabled = true;
        foreach (var c in colliders) c.enabled = true;

        IsAlive = true;
        onSpawn?.Invoke();

    }
    
    public void AddHealth(float amount)
    {
        currentHealth += amount;
        onHealthChanged?.Invoke(currentHealth);
    }


}
