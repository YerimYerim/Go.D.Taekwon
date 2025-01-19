using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class GameTurnManager : Singleton<GameTurnManager>
{   
    private int _actionPoint;
    private readonly BTNodeSelector _root = new();

    /// <summary>
    /// 턴을 실행할때마다 실행할 액션을 저장하는 스택
    /// state + 지연시간
    /// </summary>
    private Queue<TurnState>  _turnStack = new();

    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn,
        Draw,
        GameOver,
        MapClear
    }

    protected override void Awake()
    {
        base.Awake();
        TurnNodeInit();
    }

    private void Clear()
    {
        _root.Clear();   
    }
    
    public void TurnNodeInit()  
    {
        _root.Clear();
        // player Hp <= 0 인가
        var actIsDead = new BTNodeIsDead();
        // 게임 종료 action
        var actGameOver = new BTNodeGameOver();
        // 적들 중에 AP <= 0  이하인게 하나 이상 있는가.
        var actIsEnemyTurn = new BTNodeIsEnemyTurn();
        // 드로우 할건가
        var actIsDraw = new BTNodeIsDraw();
        // 유저 선택 차례
        var actPlayerTurn = new BTNodePlayerTurn();
        // 모든 적을 죽였는가
        var mapClear = new BTNodeIsMapClear();

        actIsDead.AddChild(actGameOver);

        _root.AddChild(mapClear);
        _root.AddChild(actIsDead);
        _root.AddChild(actIsEnemyTurn);
        _root.AddChild(actIsDraw);
        _root.AddChild(actPlayerTurn);
        
    }
    public void TurnStart()
    {
        _root.Evaluate();
        
    }
    
    public void AddTurnStack(TurnState turnState)
    {
        //_turnStack.Enqueue(turnState);
    }
}
