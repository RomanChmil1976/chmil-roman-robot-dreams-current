using System.Collections.Generic;

public class SequenceNode : BTNode
{
    private List<BTNode> children = new List<BTNode>();

    public SequenceNode() {}

    public SequenceNode(List<BTNode> nodes)
    {
        children = nodes;
    }

    public void AddChild(BTNode node)
    {
        children.Add(node);
    }

    public override NodeState Tick()
    {
        foreach (var child in children)
        {
            var result = child.Tick();
            if (result != NodeState.Success)
                return result;
        }
        return NodeState.Success;
    }
}