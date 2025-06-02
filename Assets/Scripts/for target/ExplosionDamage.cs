using UnityEngine;
using System.Collections.Generic;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float damage = 100f;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        HashSet<Target> damagedTargets = new HashSet<Target>();

        foreach (var col in colliders)
        {
            Target target = col.GetComponentInParent<Target>();
            if (target != null && damagedTargets.Add(target))
            {
                target.TakeDamage(damage);
            }
        }
    }
}