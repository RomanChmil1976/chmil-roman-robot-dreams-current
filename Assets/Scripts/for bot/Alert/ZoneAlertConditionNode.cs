using UnityEngine;

public class ZoneAlertConditionNode : BTNode
{
    private Transform player;
    private Vector2[] zonePolygon;

    public ZoneAlertConditionNode(Transform player, Transform[] perimeterPoints)
    {
        this.player = player;

        zonePolygon = new Vector2[perimeterPoints.Length];
        for (int i = 0; i < perimeterPoints.Length; i++)
        {
            Vector3 pos = perimeterPoints[i].position;
            zonePolygon[i] = new Vector2(pos.x, pos.z);
        }
    }

    public override NodeState Tick()
    {
        Target playerTarget = player.GetComponent<Target>();
        if (player == null || playerTarget == null || !playerTarget.IsAlive)
            return NodeState.Failure;

        Vector2 playerPos2D = new Vector2(player.position.x, player.position.z);
        return IsPointInPolygon(playerPos2D, zonePolygon) ? NodeState.Success : NodeState.Failure;
    }


    private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int j = polygon.Length - 1;
        bool inside = false;

        for (int i = 0; i < polygon.Length; j = i++)
        {
            if ((polygon[i].y > point.y) != (polygon[j].y > point.y) &&
                point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x)
            {
                inside = !inside;
            }
        }
        return inside;
    }
}