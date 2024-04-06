using System.Collections.Generic;

public abstract class BTNodeBase
{
    protected List<BTNodeBase> children = new();

    public enum State
    {
        Running,
        Success,
        Failure,
    }
    public void AddChild(BTNodeBase nodeBase)
    {
        children.Add(nodeBase);
    }

    public abstract State Evaluate();

    public void Clear()
    {
        children.Clear();
    }
}
