using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public Sprite musicOnIcon;  // Иконка "звук включен"
    public Sprite musicOffIcon; // Иконка "звук выключен"
    private Image buttonImage;   // Ссылка на Image кнопки
    private bool isMuted = false; // Флаг состояния звука
    private AudioSource audioSource;

    private void Start()
    {
        buttonImage = GetComponent<Image>();

        if (buttonImage == null)
        {
            Debug.LogError("❌ Ошибка: Компонент Image не найден на кнопке!");
            enabled = false;
            return;
        }

        audioSource = FindObjectOfType<BackgroundMusic>()?.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("⚠ Нет BackgroundMusic — отключаем MusicToggle");
            enabled = false;
            return;
        }

        isMuted = audioSource.mute;
        UpdateButtonIcon();
    }


    public void ToggleMusic()
    {
        if (audioSource == null) return;

        isMuted = !isMuted; // Инвертируем состояние
        audioSource.mute = isMuted; // Включаем или отключаем звук

        // Выводим сообщение в консоль
        Debug.Log(isMuted ? "🔇 Музыка заглушена (Mute)!" : "🔊 Музыка включена (Unmute)!");

        // Обновляем иконку
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (buttonImage == null) return;

        buttonImage.sprite = isMuted ? musicOffIcon : musicOnIcon;
    }
}