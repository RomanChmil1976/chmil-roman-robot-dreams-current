using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float damage = 100f;

    private void Start()
    {
        // Найти всех, кто попал в радиус взрыва
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var col in colliders)
        {
            Target target = col.GetComponentInParent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        // Можно визуализировать радиус (необязательно)
        Debug.DrawRay(transform.position, Vector3.up * 2, Color.red, 2f);
    }
}