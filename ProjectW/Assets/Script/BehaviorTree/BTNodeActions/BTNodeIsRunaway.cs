using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsRunaway : BTNodeAction
{
    public override State Evaluate()
    {
        Debug.Log("BTNodeIsRunaway");
        return State.Failure;
    }
}
