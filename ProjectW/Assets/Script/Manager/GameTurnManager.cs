using Script.Manager;

public class GameTurnManager : Singleton<GameTurnManager>
{
    private int _actionPoint;
    private BTNodeSelector _root;
    
    private void TurnNodeInit()
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
        // 드로우
        var actDraw = new BTNodeDraw();
        // 유저 선택
        var actPlayerTurn = new BTNodePlayerTurn();

        _root.AddChild(actIsDead);
        _root.AddChild(actIsEnemyTurn);
        _root.AddChild(actIsDraw);
        _root.AddChild(actPlayerTurn);
        
        actIsDead.AddChild(actGameOver);
        actIsEnemyTurn.AddChild(actEnemyTurnSequence);
        actEnemyTurnSequence.AddChild(actEnemyTurnAttack);
        actEnemyTurnSequence.AddChild(actIsRunaway);
        actIsRunaway.AddChild(actSelectedRunaway);
        actIsDraw.AddChild(actDraw);
    }
    private void TurnStart()
    {
        
    }
}
