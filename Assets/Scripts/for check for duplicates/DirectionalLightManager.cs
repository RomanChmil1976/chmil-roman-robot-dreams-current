using UnityEngine;

public class DirectionalLightManager : MonoBehaviour
{
    private static DirectionalLightManager instance;
    private Light directionalLight;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Получаем компонент Light
        directionalLight = GetComponent<Light>();
        if (directionalLight == null)
        {
            Debug.LogError("❌ Ошибка: На Directional Light нет компонента Light!");
        }

        // Загружаем сохраненную яркость
        float brightness = PlayerPrefs.GetFloat("DayNightLevel", 100f) / 100f;
        SetBrightness(brightness);
    }

    public void SetBrightness(float brightness)
    {
        if (directionalLight != null)
        {
            directionalLight.intensity = brightness;
        }
    }
}