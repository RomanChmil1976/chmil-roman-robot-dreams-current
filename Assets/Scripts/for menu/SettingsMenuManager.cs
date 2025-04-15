using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public Slider backgroundSoundSlider;
    public Slider commonSoundSlider;
    public Slider dayNightSlider; // Управление освещением

    private BackgroundMusic backgroundMusic;
    private DirectionalLightManager lightManager;

    private void Start()
    {
        backgroundMusic = FindObjectOfType<BackgroundMusic>();
        lightManager = FindObjectOfType<DirectionalLightManager>();

        if (backgroundMusic == null)
            Debug.LogWarning("⚠ BackgroundMusic не найден — будет без музыки");

        if (lightManager == null)
            Debug.LogWarning("⚠ DirectionalLightManager не найден — будет без управления светом");

        // Продолжаем инициализацию
        backgroundSoundSlider.value = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        commonSoundSlider.value = PlayerPrefs.GetFloat("CommonVolume", 1f);
        dayNightSlider.value = PlayerPrefs.GetFloat("DayNightLevel", 100f);

        backgroundSoundSlider.onValueChanged.AddListener(delegate { UpdateBackgroundVolume(); });
        dayNightSlider.onValueChanged.AddListener(delegate { UpdateDayNightLevel(); });

        UpdateBackgroundVolume();
        UpdateDayNightLevel();
    }


    public void UpdateBackgroundVolume()
    {
        if (backgroundSoundSlider == null) return;
        
        float volume = backgroundSoundSlider.value;
        PlayerPrefs.SetFloat("BackgroundVolume", volume);
        PlayerPrefs.Save();

        if (backgroundMusic != null)
        {
            backgroundMusic.SetVolume(volume);
        }

        Debug.Log($"🎵 Громкость фона изменена: {volume}");
    }

    public void UpdateDayNightLevel()
    {
        if (dayNightSlider == null) return;

        float brightness = dayNightSlider.value / 1f; // Преобразуем в 0-1
        PlayerPrefs.SetFloat("DayNightLevel", dayNightSlider.value);
        PlayerPrefs.Save();

        if (lightManager != null)
        {
            lightManager.SetBrightness(brightness);
        }

        Debug.Log($"🌞 Уровень освещенности: {dayNightSlider.value}/1");
    }

    public void BackToMenu()
    {
        Debug.Log("🔙 Возвращаемся в главное меню...");
        SceneManager.LoadScene("Scene_1_MainMenu");
    }
}
