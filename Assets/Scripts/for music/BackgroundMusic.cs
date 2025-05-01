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

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            AudioClip backgroundMusic = Resources.Load<AudioClip>("Audio/audio");
            if (backgroundMusic != null)
            {
                audioSource.clip = backgroundMusic;
                audioSource.loop = true;
                audioSource.playOnAwake = false;
                
                audioSource.volume = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
                
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {

            }
        }
        else
        {
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
        }
    }

    public void ToggleMute()
    {
        if (audioSource != null)
        {
            audioSource.mute = !audioSource.mute;
        }
    }

    public bool IsMuted()
    {
        return audioSource != null && audioSource.mute;
    }
}
