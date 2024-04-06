public class BTNodeSequence : BTNodeBase
{
    public override State Evaluate()
    {
        bool isAnyChildRunning = false;

        foreach (var node in children)
        {
            State result = node.Evaluate();
            switch (result)
            {
                case State.Failure:
                    return State.Failure;
                case State.Running:
                    isAnyChildRunning = true;
                    break;
            }
        }
        return isAnyChildRunning ? State.Running : State.Success;
    }
}
