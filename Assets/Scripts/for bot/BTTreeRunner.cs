using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTreeRunner : MonoBehaviour
{
    [System.Serializable]
    public class BotData
    {
        public Transform botTransform;
        public NavMeshAgent agent;
        public BotVisualHandler visualHandler;
    }

    [Header("Global Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float alertDistance = 5f;
    [SerializeField] private float zoneTriggerRadius = 10f;
   

    [Header("Bots")]
    [SerializeField] private BotData[] bots;
    
    [Header("Siren Settings")]
    [SerializeField] private AudioSource sirenAudioSource;
    [SerializeField] private AudioClip sirenClip;
    [SerializeField, Range(0f, 1f)] private float sirenVolume = 0.5f;

    [Header("Bullet Settings")]
    [SerializeField] private GameObject playerBulletPrefab; 
    [SerializeField] private GameObject bulletPrefab;   

    private List<BTNode> botTrees = new List<BTNode>();
    private Transform player;
    private bool isSirenPlaying = false;
    private bool anyBotInAlert = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        foreach (var bot in bots)
        {
            if (bot.botTransform == null || bot.agent == null) continue;
            if (!bot.agent.gameObject.activeInHierarchy || !bot.agent.isOnNavMesh) continue;

            var patrolNode = new PatrolNode(bot.botTransform, bot.agent, patrolPoints, stoppingDistance, bot.visualHandler);
            var proximityAlert = new AlertConditionNode(bot.botTransform, player, alertDistance);
            var zoneAlert = new ZoneAlertConditionNode(player, patrolPoints);
            var chaseNode = new ChasePlayerNode(bot.botTransform, bot.agent, player, stoppingDistance, bot.visualHandler);
            var alertSelector = new SelectorNode(new List<BTNode> { proximityAlert, zoneAlert });

            SequenceNode alertSequence;

            if (bot.botTransform.name.Contains("Bot_Combat"))
            {
                Transform firePoint = bot.botTransform.GetChild(0).Find("FirePoint");

                var shootNode = new BotCombatAttackNode(
                    bot.botTransform,
                    player,
                    bot.agent,
                    bot.botTransform.GetComponent<Animator>(),
                    bulletPrefab,
                    firePoint,
                    40f, 
                    stoppingDistance 
                );

                alertSequence = new SequenceNode(new List<BTNode> { alertSelector, chaseNode, shootNode });
            }
            else
            {
                alertSequence = new SequenceNode(new List<BTNode> { alertSelector, chaseNode });
            }

            var rootSelector = new SelectorNode(new List<BTNode> { alertSequence, patrolNode });
            botTrees.Add(rootSelector);
        }
    }

    private void Update()
    {
        anyBotInAlert = false;

        foreach (var bot in bots)
        {
            if (bot == null || bot.botTransform == null) continue; 

            bool isProximityAlert = Vector3.Distance(bot.botTransform.position, player.position) < alertDistance;
            bool isZoneAlert = IsPlayerInZone();

            if (isProximityAlert || isZoneAlert)
            {
                anyBotInAlert = true;
                break;
            }
        }

        if (anyBotInAlert && !isSirenPlaying)
        {
            PlaySiren();
        }
        else if (!anyBotInAlert && isSirenPlaying)
        {
            StopSiren();
        }

        foreach (var tree in botTrees)
        {
            tree.Tick();
        }
    }

    private bool IsPlayerInZone()
    {
        if (patrolPoints.Length == 0 || player == null) return false;

        Vector2 playerPos2D = new Vector2(player.position.x, player.position.z);
        Vector2[] zonePolygon = new Vector2[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Vector3 pos = patrolPoints[i].position;
            zonePolygon[i] = new Vector2(pos.x, pos.z);
        }

        int j = zonePolygon.Length - 1;
        bool inside = false;
        for (int i = 0; i < zonePolygon.Length; j = i++)
        {
            if ((zonePolygon[i].y > playerPos2D.y) != (zonePolygon[j].y > playerPos2D.y) &&
                playerPos2D.x < (zonePolygon[j].x - zonePolygon[i].x) * (playerPos2D.y - zonePolygon[i].y) / (zonePolygon[j].y - zonePolygon[i].y) + zonePolygon[i].x)
            {
                inside = !inside;
            }
        }
        return inside;
    }

    private void PlaySiren()
    {
        if (sirenAudioSource != null && sirenClip != null)
        {
            sirenAudioSource.clip = sirenClip;
            sirenAudioSource.volume = sirenVolume;
            sirenAudioSource.loop = true;
            sirenAudioSource.Play();
            isSirenPlaying = true;
        }
    }

    private void StopSiren()
    {
        if (sirenAudioSource != null)
        {
            sirenAudioSource.Stop();
            isSirenPlaying = false;
        }
    }
}
