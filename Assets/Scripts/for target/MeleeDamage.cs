using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponent<Target>();
        if (target != null && target.CompareTag("Player"))
        {
            target.TakeDamage(damage);
        }
    }
}