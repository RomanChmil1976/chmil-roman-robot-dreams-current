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
        float distance = Vector3.Distance(bot.position, player.position);
        return distance < alertDistance ? NodeState.Success : NodeState.Failure;
    }
}