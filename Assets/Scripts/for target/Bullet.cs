using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 25f;

    private void OnCollisionEnter(Collision collision)
    {
        Target target = collision.collider.GetComponentInParent<Target>();
        
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // if (target.CompareTag("Player"))
        //     target.TakeDamage(10f); 

        
        Destroy(gameObject);
    }
}