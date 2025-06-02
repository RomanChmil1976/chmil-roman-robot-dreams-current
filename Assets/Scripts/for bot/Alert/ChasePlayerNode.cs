using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerNode : BTNode
{
    private Transform bot;
    private Transform player;
    private NavMeshAgent agent;
    private float stoppingDistance;
    private BotVisualHandler visuals;
    private Animator anim;

    //public ChasePlayerNode(Transform bot, NavMeshAgent agent, Transform player, BotVisualHandler visuals)
    public ChasePlayerNode(Transform bot, NavMeshAgent agent, Transform player, float stoppingDistance, BotVisualHandler visuals)

    {
        this.bot = bot;
        this.agent = agent;
        this.player = player;
        this.visuals = visuals;
        //this.stoppingDistance = 1.5f;
        //this.stoppingDistance = agent.stoppingDistance;
        this.stoppingDistance = stoppingDistance;

        
        anim = bot.GetComponent<Animator>();
        //Animator anim = bot.GetComponent<Animator>();
        //anim = bot.GetComponentInChildren<Animator>();


    }

    public override NodeState Tick()
    {
        Target playerTarget = player.GetComponent<Target>();
        if (player == null || playerTarget == null || !playerTarget.IsAlive)
            return NodeState.Failure;

        visuals?.SetAlertVisuals(true);
        agent.speed = 4.5f;

        if (anim != null && anim.runtimeAnimatorController != null)
        {
            float speed = agent.velocity.magnitude;
            anim.SetFloat("Speed", speed);
            Debug.Log($"[{bot.name}] Chase Speed: {speed:F2}");
        }
    
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(bot.position, player.position);
        return distance < stoppingDistance ? NodeState.Success : NodeState.Running;
    }
}