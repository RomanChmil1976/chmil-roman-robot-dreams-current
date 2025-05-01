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
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            enabled = false;
            return;
        }

        audioSource = FindObjectOfType<BackgroundMusic>()?.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            enabled = false;
            return;
        }
        
        UpdateButtonIcon();
    }

    public void ToggleMusic()
    {
        if (audioSource == null) return;

        audioSource.mute = !audioSource.mute;

        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (buttonImage == null) return;

        buttonImage.sprite = audioSource.mute ? musicOffIcon : musicOnIcon;
    }
}