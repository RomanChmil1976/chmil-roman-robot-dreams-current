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
    [SerializeField] private GameObject bulletPrefab;

    [Header("Bots")]
    [SerializeField] private BotData[] bots;

    private List<BTNode> botTrees = new List<BTNode>();
    private Transform player;

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
                //Transform firePoint = bot.botTransform.Find("FirePoint");
                Transform firePoint = bot.botTransform.GetChild(0).Find("FirePoint");

                var shootNode = new BotCombatAttackNode(
                    bot.botTransform,
                    player,
                    bot.agent,
                    bot.botTransform.GetComponent<Animator>(),
                    bulletPrefab,
                    firePoint,
                    40f,               // bullet speed
                    stoppingDistance   // shooting distance
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
        foreach (var tree in botTrees)
        {
            tree.Tick();
        }
    }
}
