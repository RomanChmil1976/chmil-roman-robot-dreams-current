using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public Camera cam;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void Update()
    {
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.transform.forward);
        }
    }

    public void SetHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}