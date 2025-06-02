using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : BTNode
{
    private Transform bot;
    private Transform[] patrolPoints;
    private float stoppingDistance;
    private NavMeshAgent agent;
    private int currentPointIndex;
    private float stuckTimer = 0f;
    private float stuckThreshold = 2f;
    private BotVisualHandler visuals;
    private Animator anim;

    public PatrolNode(Transform botTransform, NavMeshAgent agent, Transform[] points, float stopDist, BotVisualHandler visuals)
    {
        bot = botTransform;
        patrolPoints = points;
        this.agent = agent;
        stoppingDistance = stopDist;
        this.visuals = visuals;

        anim = bot.GetComponent<Animator>();

        currentPointIndex = Random.Range(0, patrolPoints.Length);
        if (agent != null && patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    public override NodeState Tick()
    {
        if (agent == null || patrolPoints.Length == 0)
            return NodeState.Failure;

        if (!agent.hasPath || agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.Log($"[{bot.name}] Path incomplete or missing. Resetting path.");
            currentPointIndex = Random.Range(0, patrolPoints.Length);
            agent.SetDestination(patrolPoints[currentPointIndex].position);
            stuckTimer = 0f;
        }

        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            int newIndex;
            do {
                newIndex = Random.Range(0, patrolPoints.Length);
            } while (newIndex == currentPointIndex);
            currentPointIndex = newIndex;
            agent.SetDestination(patrolPoints[currentPointIndex].position);
            stuckTimer = 0f;
        }

        if (agent.velocity.sqrMagnitude < 0.05f) 
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > stuckThreshold)
            {
                Debug.Log($"[{bot.name}] Bot seems stuck. Changing destination.");
                currentPointIndex = Random.Range(0, patrolPoints.Length);
                agent.SetDestination(patrolPoints[currentPointIndex].position);
                stuckTimer = 0f;
            }
        }
        else stuckTimer = 0f;

        visuals?.SetAlertVisuals(false);
        agent.speed = 2.0f;

        if (anim != null && anim.runtimeAnimatorController != null)
        {
            float speed = agent.velocity.magnitude;
            anim.SetFloat("Speed", speed);
            Debug.Log($"[{bot.name}] Patrol Speed: {speed:F2}");
        }

        return NodeState.Running;
    }
}
