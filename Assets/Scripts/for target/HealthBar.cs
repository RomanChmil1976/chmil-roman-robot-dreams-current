using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public Camera cam;

    private Target target;

    private void Awake()
    {
        TargetManager.onTargetSpawn += OnTargetSpawn;
        TargetManager.onTargetDespawn += OnTargetDespawn;
    }

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;

        target = GetComponentInParent<Target>();
        if (target != null && target.gameObject.activeInHierarchy)
        {
            OnTargetSpawn(target);
        }
    }

    private void OnDestroy()
    {
        TargetManager.onTargetSpawn -= OnTargetSpawn;
        TargetManager.onTargetDespawn -= OnTargetDespawn;

        if (target != null)
        {
            target.onHealthChanged -= UpdateBar;
            target.OnDeath -= OnTargetDeath;
        }
    }

    private void OnTargetSpawn(Target spawnedTarget)
    {
        if (spawnedTarget == GetComponentInParent<Target>())
        {
            target = spawnedTarget;

            gameObject.SetActive(true);
            target.onHealthChanged += UpdateBar;
            target.OnDeath += OnTargetDeath;

            UpdateBar((int)target.maxHealth);
        }
    }

    private void OnTargetDespawn(Target despawnedTarget)
    {
        if (despawnedTarget == target)
        {
            target.onHealthChanged -= UpdateBar;
            target.OnDeath -= OnTargetDeath;
            target = null;

            gameObject.SetActive(false);
        }
    }

    private void OnTargetDeath()
    {
        gameObject.SetActive(false);
    }

    private void UpdateBar(int hp)
    {
        if (fillImage != null)
            fillImage.fillAmount = hp / 100f;
    }

    private void Update()
    {
        if (cam != null)
            transform.LookAt(transform.position + cam.transform.forward);
    }
    
    public void SetHealth(float current, float max)
    {
        if (fillImage != null && max > 0f)
            fillImage.fillAmount = Mathf.Clamp01(current / max);
    }
}