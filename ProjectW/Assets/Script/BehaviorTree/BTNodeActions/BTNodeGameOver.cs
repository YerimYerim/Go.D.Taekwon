using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeGameOver : BTNodeAction
{
    public override State Evaluate()
    {
        return State.Failure;
    }
}
