using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsDead : BTNodeAction
{
    
    public override State Evaluate()
    {
        return State.Failure;
    }
}
