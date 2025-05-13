using System.Collections.Generic;

public class SelectorNode : BTNode
{
    private List<BTNode> children = new List<BTNode>();

    public SelectorNode() {}

    public SelectorNode(List<BTNode> nodes)
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
            if (result != NodeState.Failure)
                return result;
        }
        return NodeState.Failure;
    }
}