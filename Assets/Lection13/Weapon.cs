using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint; 
    public GameObject bulletPrefab; 
    public float bulletForce = 1100f; 
    public float fireRate = 0.2f; 
    private float nextFireTime = 0f;

    void Start()
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Debug.Log(""); 

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Debug.Log("" + bullet.name); 

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firePoint.forward * bulletForce, ForceMode2D.Impulse);
                Debug.Log("" + (firePoint.up * bulletForce));
            }

            Destroy(bullet, 3f); 
        }
        else
        {
            Debug.LogWarning("Bullet prefab or fire point not assigned!");
        }
    }
}