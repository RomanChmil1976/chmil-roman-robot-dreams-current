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

    public event Action onSpawn;
    public event Action<float> onHealthChanged;
    public event Action OnDeath;
    
    public float CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

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


        if (currentHealth <= 0f)
            StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        DisableTarget();
        OnDeath?.Invoke();

        yield return new WaitForSecondsRealtime(respawnTime);
        Respawn();
    }

    private void DisableTarget()
    {
        foreach (var r in renderers) r.enabled = false;
        foreach (var c in colliders) c.enabled = false;
    }

    private void Respawn()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentHealth = maxHealth;

        foreach (var r in renderers) r.enabled = true;
        foreach (var c in colliders) c.enabled = true;

        onSpawn?.Invoke();
        
        // var visualHandler = GetComponent<BotVisualHandler>();
        // if (visualHandler != null)
        // {
        //     visualHandler.SetAlertVisuals(false);
        // }

    }
}
