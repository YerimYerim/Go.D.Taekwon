using Script.Manager;
using UnityEngine;
using UnityEngine.Serialization;

public class GameTurnManager : Singleton<GameTurnManager>
{   
    private int _actionPoint;
    private BTNodeSelector _root = new BTNodeSelector();
    public bool IsMyTurn = false;

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
        // EnemyTurn selector 
        var actEnemyTurnSequence = new BTNodeSequence();
        // 적의 공격
        var actEnemyTurnAttack = new BTNodeEnemyAttack();
        // 도망을 선택한 적이 있는가
        var actIsRunaway = new BTNodeIsRunaway();
        // 도망
        var actSelectedRunaway = new BTNodeSelectedRunaway();
        // 드로우 할건가
        var actIsDraw = new BTNodeIsDraw();
        // 유저 선택
        var actPlayerTurn = new BTNodePlayerTurn();
        // 모든 적을 죽였는간
        var mapClear = new BTNodeIsMapClear();

        _root.AddChild(mapClear);
        _root.AddChild(actIsDead);
        _root.AddChild(actIsEnemyTurn);
        _root.AddChild(actIsDraw);
        _root.AddChild(actPlayerTurn);
        
        actIsDead.AddChild(actGameOver);
        actIsEnemyTurn.AddChild(actEnemyTurnSequence);
        actEnemyTurnSequence.AddChild(actEnemyTurnAttack);
        actEnemyTurnSequence.AddChild(actIsRunaway);
        actIsRunaway.AddChild(actSelectedRunaway);
    }
    public void TurnStart()
    {
        _root.Evaluate();
        GameBattleManager.Instance.DoTurn();
    }

    public void DoActionEnemy()
    {
        
    }
}
