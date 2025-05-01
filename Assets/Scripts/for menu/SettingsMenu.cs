using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public Slider backgroundSoundSlider;
    public Slider dayNightSlider;

    private BackgroundMusic backgroundMusic;
    private DirectionalLightManager lightManager;

    private void Start()
    {
        backgroundMusic = FindObjectOfType<BackgroundMusic>();
        lightManager = FindObjectOfType<DirectionalLightManager>();

        if (backgroundSoundSlider != null)
        {
            backgroundSoundSlider.onValueChanged.RemoveAllListeners();
            backgroundSoundSlider.value = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
            backgroundSoundSlider.onValueChanged.AddListener(_ => UpdateBackgroundVolume());
        }

        if (dayNightSlider != null)
        {
            dayNightSlider.onValueChanged.RemoveAllListeners();
            dayNightSlider.value = PlayerPrefs.GetFloat("DayNightLevel", 100f);
            dayNightSlider.onValueChanged.AddListener(_ => UpdateDayNightLevel());
        }

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
    }

    public void UpdateDayNightLevel()
    {
        if (dayNightSlider == null) return;

        float brightness = dayNightSlider.value / 1f;
        PlayerPrefs.SetFloat("DayNightLevel", dayNightSlider.value);
        PlayerPrefs.Save();

        if (lightManager != null)
        {
            lightManager.SetBrightness(brightness);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Scene_1_MainMenu");
    }
}