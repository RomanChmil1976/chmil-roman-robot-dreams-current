public enum NodeState { Running, Success, Failure }

public abstract class BTNode
{
    public abstract NodeState Tick();
}