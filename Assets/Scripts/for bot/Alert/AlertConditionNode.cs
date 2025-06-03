using UnityEngine;

public class AlertConditionNode : BTNode
{
    private Transform bot;
    private Transform player;
    private float alertDistance;

    public AlertConditionNode(Transform bot, Transform player, float alertDistance)
    {
        this.bot = bot;
        this.player = player;
        this.alertDistance = alertDistance;
    }

    public override NodeState Tick()
    {
        if (bot == null)
        {
            Debug.LogWarning("AlertConditionNode: Bot Transform has been destroyed or is missing.");
            return NodeState.Failure;
        }

        if (player == null)
        {
            Debug.LogWarning("AlertConditionNode: Player Transform has been destroyed or is missing.");
            return NodeState.Failure;
        }

        Target playerTarget = player.GetComponent<Target>();
        if (playerTarget == null)
        {
            Debug.LogWarning("AlertConditionNode: Player Target component is missing.");
            return NodeState.Failure;
        }

        if (!playerTarget.IsAlive)
        {
            Debug.Log("AlertConditionNode: Player is not alive.");
            return NodeState.Failure;
        }

        float distance = Vector3.Distance(bot.position, player.position);
        if (distance < alertDistance)
        {
            return NodeState.Success;
        }
        else
        {
            return NodeState.Failure;
        }
    }
}