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

    private List<BTNode> botTrees = new List<BTNode>();
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        if (player == null)
            Debug.LogError("BTTreeRunner: Player not found!");
        else
            Debug.Log("BTTreeRunner: Player found -> " + player.name);


        foreach (var bot in bots)
        {
            if (bot.botTransform == null || bot.agent == null) continue;

            var patrolNode = new PatrolNode(bot.botTransform, bot.agent, patrolPoints, stoppingDistance, bot.visualHandler);
            var proximityAlert = new AlertConditionNode(bot.botTransform, player, alertDistance);
            var zoneAlert = new ZoneAlertConditionNode(player, patrolPoints);
            var chaseNode = new ChasePlayerNode(bot.botTransform, bot.agent, player, bot.visualHandler);

            // Бот реагирует либо на близость, либо на вход в зону
            var alertSelector = new SelectorNode(new List<BTNode> { proximityAlert, zoneAlert });

            var alertSequence = new SequenceNode(new List<BTNode> { alertSelector, chaseNode });

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