using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private Sprite musicOnIcon;
    [SerializeField] private Sprite musicOffIcon;

    private Image buttonImage;
    private AudioSource audioSource;

    private void Awake()
    {
        // Получаем Image с кнопки
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("❌ MusicToggle: Image component not found!");
            enabled = false;
            return;
        }

        // Автоматически ищем BackgroundMusic → AudioSource
        audioSource = FindObjectOfType<BackgroundMusic>()?.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("⚠ MusicToggle: BackgroundMusic not found!");
            enabled = false;
            return;
        }

        // Сразу обновляем иконку по текущему состоянию
        UpdateButtonIcon();
    }

    public void ToggleMusic()
    {
        if (audioSource == null) return;

        audioSource.mute = !audioSource.mute;
        Debug.Log(audioSource.mute ? "🔇 Music muted" : "🔊 Music unmuted");
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (buttonImage == null) return;

        buttonImage.sprite = audioSource.mute ? musicOffIcon : musicOnIcon;
    }
}