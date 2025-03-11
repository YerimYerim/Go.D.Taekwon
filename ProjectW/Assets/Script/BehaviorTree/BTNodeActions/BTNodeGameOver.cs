using Script.Manager;
using UnityEngine;

public class BTNodeGameOver : BTNodeAction
{
    public override State Evaluate()
    {
        Debug.Log("IS Game Over");
        GameTurnManager.Instance.AddTurnStack(GameTurnManager.TurnState.GameOver);
        if (GameUIManager.Instance.TryGetOrCreate<UI_PopUp_Die>(false, UILayer.LEVEL_3, out var ui))
        {
            ui.Show();
        }
        return State.Failure;
    }
}
