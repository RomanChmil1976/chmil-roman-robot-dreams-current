using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public Camera cam;

    private Target target;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        target = GetComponentInParent<Target>();
        if (target != null)
        {
            target.onSpawn += OnTargetSpawn;
            target.onHealthChanged += UpdateBar;
            target.OnDeath += OnTargetDeath;
        }
    }

    private void OnTargetSpawn()
    {
        gameObject.SetActive(true);
    }

    private void OnTargetDeath()
    {
        gameObject.SetActive(false);
        if (target != null)
        {
            target.onHealthChanged -= UpdateBar;
            target.OnDeath -= OnTargetDeath;
            target.onSpawn += OnTargetSpawn;
        }
    }

    private void UpdateBar(int hp)
    {
        if (fillImage != null)
            fillImage.fillAmount = hp / 100f;
    }

    void Update()
    {
        if (cam != null)
            transform.LookAt(transform.position + cam.transform.forward);
    }

    public void SetHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}