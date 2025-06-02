using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25f;
    public float lifeTime = 5f;

    public enum BulletOwner { Player, Bot };
    public BulletOwner owner;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Target target = collision.collider.GetComponentInParent<Target>();
    
        if (target != null)
        {
            if (CompareTag("BotBullet") && target.CompareTag("Player"))
            {
                target.TakeDamage(damage);
            }
            else if (CompareTag("PlayerBullet") && target.CompareTag("Bot"))
            {
                target.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

}
