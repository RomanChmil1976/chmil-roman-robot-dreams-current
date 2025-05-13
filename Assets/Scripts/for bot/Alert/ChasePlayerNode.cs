using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerNode : BTNode
{
    private Transform bot;
    private Transform player;
    private NavMeshAgent agent;
    private float stoppingDistance;
    private BotVisualHandler visuals;

    public ChasePlayerNode(Transform bot, NavMeshAgent agent, Transform player, BotVisualHandler visuals)
    {
        this.bot = bot;
        this.agent = agent;
        this.player = player;
        this.visuals = visuals;
        this.stoppingDistance = 1.5f;
    }

    public override NodeState Tick()
    {
        if (player == null || agent == null) return NodeState.Failure;

        visuals?.SetAlertVisuals(true);

        agent.SetDestination(player.position);

        float distance = Vector3.Distance(bot.position, player.position);
        return distance < stoppingDistance ? NodeState.Success : NodeState.Running;
    }
}