using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;       // откуда вылетает пуля
    [SerializeField] private Transform aimTarget;       // цель — AimTarget
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float spawnOffset = 5.0f; /// расстояние вылета пули
    
    public enum ShootingMode
    {
        Raycast,
        SphereCast
    }
    
    [Header("Shooting Settings")]
    [SerializeField] private ShootingMode shootingMode = ShootingMode.Raycast;
    [SerializeField] private float sphereCastRadius = 0.5f;
    
    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera;

   public void Fire2()
    {
        if (bulletPrefab == null || firePoint == null || playerCamera == null)
        {
            Debug.LogWarning("❌ Не хватает компонентов!");
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 direction;

        bool hasHit = false;
        RaycastHit hit = new RaycastHit();

        // Выбор режима стрельбы
        switch (shootingMode)
        {
            case ShootingMode.Raycast:
                hasHit = Physics.Raycast(ray, out hit, 100f);
                break;
            case ShootingMode.SphereCast:
                hasHit = Physics.SphereCast(ray, sphereCastRadius, out hit, 100f);
                break;
        }

        // Направление стрельбы
        if (hasHit)
        {
            direction = (hit.point - firePoint.position).normalized;
            Debug.Log($"🎯 Hit {hit.collider.name} at {hit.point}");
        }
        else
        {
            Vector3 targetPoint = ray.GetPoint(100f);
            direction = (targetPoint - firePoint.position).normalized;
            Debug.Log("❌ Missed. Shooting forward.");
        }

        // Точка вылета пули
        Vector3 spawnPosition = firePoint.position + direction * 0.1f;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.velocity = direction * bulletSpeed;
        }

        Destroy(bullet, lifeTime);
        Debug.DrawRay(firePoint.position, direction * 20f, Color.cyan, 2f);
    }

}