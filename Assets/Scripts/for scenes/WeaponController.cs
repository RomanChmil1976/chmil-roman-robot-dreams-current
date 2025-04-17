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
    
    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera;

                                                       
    public void Fire2()
    {
        if (bulletPrefab == null || firePoint == null || playerCamera == null)
        {
            Debug.LogWarning("❌ Не хватает компонентов!");
            return;
        }

        // Луч из центра экрана
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 direction;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // Стреляем ТУДА, КУДА СМОТРИТ ПРИЦЕЛ
            direction = (hit.point - firePoint.position).normalized;
        }
        else
        {
            // Иначе просто по направлению взгляда
            Vector3 targetPoint = ray.GetPoint(100f);
            direction = (targetPoint - firePoint.position).normalized;
        }

        // Смещаем точку вылета немного вперёд от оружия
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
        Debug.DrawRay(firePoint.position, direction * 20f, Color.magenta, 2f);
    }


}