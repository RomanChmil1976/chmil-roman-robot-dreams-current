using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDamageOverlay : MonoBehaviour
{
    [SerializeField] private Image overlayImage;         
    [SerializeField] private float overlayDuration = 0.5f; 
    [SerializeField] private Sprite bloodSprite;      
    [SerializeField] private float overlayAlpha = 0.8f;   

    [Header("Sound Settings")]
    [SerializeField] private AudioClip damageSound;  
    [SerializeField] private AudioSource audioSource; 

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (overlayImage != null)
        {
            overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, 0f);
        }
    }

    public void ShowDamageOverlay()
    {
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOverlay());
    }

    private IEnumerator FadeOverlay()
    {
        if (overlayImage != null)
        {
            if (bloodSprite != null)
                overlayImage.sprite = bloodSprite;

            Color startColor = overlayImage.color;
            startColor.a = overlayAlpha;
            overlayImage.color = startColor;

            yield return new WaitForSeconds(overlayDuration);

            float elapsed = 0f;
            float fadeTime = overlayDuration; 
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                Color fadeColor = overlayImage.color;
                fadeColor.a = Mathf.Lerp(overlayAlpha, 0f, elapsed / fadeTime);
                overlayImage.color = fadeColor;
                yield return null;
            }
        }
    }
}