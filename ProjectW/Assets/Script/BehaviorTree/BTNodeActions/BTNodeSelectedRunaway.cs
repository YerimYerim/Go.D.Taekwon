public class BTNodeSelectedRunaway : BTNodeAction
{
    public override State Evaluate()
    {
        return State.Failure;
    }
}