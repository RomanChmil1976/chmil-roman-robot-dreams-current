using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Пуля попала в " + other.name);

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Удаляем врага
        }

        Destroy(gameObject); // Удаляем пулю
    }
}