using UnityEngine;
using UnityEngine.AI;

public class BotCombatAttackNode : BTNode
{
    private Transform bot;
    private Transform player;
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject bulletPrefab;
    private Transform firePoint;
    private float bulletSpeed;
    private float shootingDistance;
    private float shootCooldown = 5f;
    private float lastShotTime = -Mathf.Infinity;

    public BotCombatAttackNode(Transform bot, Transform player, NavMeshAgent agent, Animator anim,
        GameObject bulletPrefab, Transform firePoint, float bulletSpeed, float shootingDistance)
    {
        this.bot = bot;
        this.player = player;
        this.agent = agent;
        this.anim = anim;
        this.bulletPrefab = bulletPrefab;
        this.firePoint = firePoint;
        this.bulletSpeed = bulletSpeed;
        this.shootingDistance = shootingDistance;
    }

    public override NodeState Tick()
    {
        if (player == null || bot == null || firePoint == null || bulletPrefab == null)
            return NodeState.Failure;

        float distance = Vector3.Distance(bot.position, player.position);

        if (distance <= shootingDistance && Time.time - lastShotTime >= shootCooldown)
        {
            Shoot();
            lastShotTime = Time.time;
        }

        return NodeState.Success;
    }

    private void Shoot()
    {
        Vector3 direction = (player.position + Vector3.up) - firePoint.position;
        GameObject bullet = Object.Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction.normalized));

        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.velocity = direction.normalized * bulletSpeed;
        }

        if (anim != null)
        {
            anim.SetTrigger("Shoot");
        }
    }
}