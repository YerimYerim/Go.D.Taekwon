public class BTNodeSelector : BTNodeBase
{
    public override State Evaluate()
    {
        foreach (var node in children)
        {
            var result = node.Evaluate();
            switch (result)
            {
                case State.Success:
                    return State.Success;
                case State.Running:
                    return State.Running;
            }
        }

        return State.Failure;
    }
}
