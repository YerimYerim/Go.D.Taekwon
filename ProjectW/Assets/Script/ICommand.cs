using System;
using UnityEngine;

public interface ICommand
{
    public void Execute()
    {
        //throw new System.NotImplementedException();
    }
}


public class PlayerTurnCommand : ICommand
{
    public event Action OnAction;
    public PlayerTurnCommand(Action action)
    {
        OnAction = action;
    } 
    public void Execute()
    {
        // Logic for player turn
        OnAction?.Invoke();
    //    Debug.Log("Player's turn.");
    }
}

public class EnemyTurnCommand : ICommand
{
    public event Action OnAction;
    public EnemyTurnCommand(Action action)
    {
        OnAction = action;
    } 
    public void Execute()
    {
        OnAction?.Invoke();
//        Debug.Log("Enemy's turn executed.");
    }
}

public class DrawCommand : ICommand
{
    public event Action OnAction;
    public DrawCommand(Action action)
    {
        OnAction = action;
    } 
    public void Execute()
    {
        // Logic for draw
        OnAction?.Invoke();
    //    Debug.Log("Draw executed.");
    }
}

public class GameOverCommand : ICommand
{
    public event Action OnAction;
    public GameOverCommand(Action action)
    {
        OnAction = action;
    } 
    
    public void Execute()
    {
        // Logic for game over
        OnAction?.Invoke();
     //   Debug.Log("Game over executed.");
    }
}

public class MapClearCommand : ICommand
{
    public event Action OnAction;
    public MapClearCommand(Action action)
    {
        OnAction = action;
    } 

    public void Execute()
    {
        // Logic for map clear
        OnAction?.Invoke();
   //     Debug.Log("Map clear executed.");
    }
}