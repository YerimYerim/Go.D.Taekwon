using UnityEngine;

public class BTNodeGameOver : BTNodeAction
{
    public override State Evaluate()
    {
        Debug.Log("IS Game Over");
        GameTurnManager.Instance.AddTurnStack(GameTurnManager.TurnState.GameOver);
        
        GameInstanceManager.Instance.BattleStart();
        return State.Failure;
    }
}
