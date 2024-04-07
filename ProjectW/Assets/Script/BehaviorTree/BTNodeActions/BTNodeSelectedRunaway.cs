using UnityEngine;

public class BTNodeSelectedRunaway : BTNodeAction
{
    public override State Evaluate()
    {
        Debug.Log("BTNodeSelectedRunaway");
        return State.Failure;
    }
}