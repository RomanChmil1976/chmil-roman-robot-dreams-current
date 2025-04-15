using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("🎵 BackgroundMusic создан!");

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                Debug.Log("🎵 AudioSource создан на BackgroundMusic!");
            }

            // Загружаем аудиофайл
            AudioClip backgroundMusic = Resources.Load<AudioClip>("Audio/audio");
            if (backgroundMusic != null)
            {
                audioSource.clip = backgroundMusic;
                audioSource.loop = true;
                audioSource.playOnAwake = false;
                
                // Загружаем сохраненную громкость
                audioSource.volume = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
                
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                    Debug.Log("🎵 Музыка запущена!");
                }
            }
            else
            {
                Debug.LogError("❌ Ошибка: Файл audio.mp3 не найден в Resources/Audio/");
            }
        }
        else
        {
            Debug.LogWarning("⚠ `BackgroundMusic` уже существует, удаляем новый экземпляр!");
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            PlayerPrefs.SetFloat("BackgroundVolume", volume);
            PlayerPrefs.Save();
            Debug.Log($"🔊 Установлена громкость фоновой музыки: {volume}");
        }
    }

    public void ToggleMute()
    {
        if (audioSource != null)
        {
            audioSource.mute = !audioSource.mute;
            Debug.Log(audioSource.mute ? "🔇 Музыка выключена!" : "🔊 Музыка включена!");
        }
    }

    public bool IsMuted()
    {
        return audioSource != null && audioSource.mute;
    }
}
