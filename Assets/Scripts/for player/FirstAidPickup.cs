using UnityEngine;

public class FirstAidPickup : MonoBehaviour
{
    public float healAmount = 20f; 
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target playerTarget = other.GetComponent<Target>();
            if (playerTarget != null)
            {
                playerTarget.AddHealth(healAmount);
                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                Destroy(gameObject);
            }
        }
    }
}