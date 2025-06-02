using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float spawnOffset = 5f;
    [SerializeField] private ShootingMode shootingMode = ShootingMode.Raycast;
    [SerializeField] private float sphereCastRadius = 0.5f;
    [SerializeField] private Camera playerCamera;

    [Header("Audio")]
    [SerializeField] private AudioClip gunShotClip;
    [SerializeField] private AudioSource audioSource;

    public enum ShootingMode { Raycast, SphereCast }

    public void Fire2()
    {
        if (bulletPrefab == null || firePoint == null || playerCamera == null)
            return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 direction;
        RaycastHit hit;
        bool hasHit = shootingMode == ShootingMode.SphereCast
            ? Physics.SphereCast(ray, sphereCastRadius, out hit, 100f)
            : Physics.Raycast(ray, out hit, 100f);

        direction = hasHit
            ? (hit.point - firePoint.position).normalized
            : (ray.GetPoint(100f) - firePoint.position).normalized;

        Vector3 spawnPosition = firePoint.position + direction * spawnOffset;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));

        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.velocity = direction * bulletSpeed;
        }

        Destroy(bullet, lifeTime);

        if (audioSource != null && gunShotClip != null)
            audioSource.PlayOneShot(gunShotClip);
    }
}