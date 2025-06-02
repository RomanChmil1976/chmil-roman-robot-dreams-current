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
        Target playerTarget = player.GetComponent<Target>();
        if (player == null || playerTarget == null || !playerTarget.IsAlive)
            return NodeState.Failure;

        float distance = Vector3.Distance(bot.position, player.position);
        return distance < alertDistance ? NodeState.Success : NodeState.Failure;
    }

}