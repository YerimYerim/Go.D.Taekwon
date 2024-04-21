using Script.Manager;
using UnityEngine;

/// <summary>
/// battlemanager
/// </summary>
public class GameBattleManager : Singleton<GameBattleManager>
{
    public (ActorDataBase data, GameActor actor) enemy = new();
    public (ActorDataBase data, GameActor actor)player = new();
    
    public int PassivePoint = 1;
    public void Init()
    {
        enemy.data = new ActorDataBase();
        enemy.data.Init(10, 5);
        enemy.actor = GameActormanager.Instance.GetActor("ActorEnemy");
        enemy.actor.OnUpdateHp(10,10);
        player.data = new ActorDataBase();
        player.data.Init(10);
        player.actor = GameActormanager.Instance.GetActor("ActorPlayer");
        player.actor.OnUpdateHp(10,10);
    }

    public int GetEnemyHp()
    {
        return enemy.data.GetHp();
    }

    public int GetMyHp()
    {
        return player.data.GetHp();
    }
    public void Attack(int damage)
    {
        if (GameTurnManager.Instance.isMyTurn == true)
        {
            enemy.data.DoDamaged(damage);
            enemy.data.MinusAP(1);
            enemy.actor.OnUpdateHp(enemy.data.MaxHp, enemy.data.Hp);
            ++PassivePoint;
            GameTurnManager.Instance.TurnStart();
        }
        else
        {
            Debug.Log("적의 턴입니다. 공격할 수 없습니다.");
        }
    }
    public void Damaged(int damage)
    {
        if (GameTurnManager.Instance.isMyTurn == false)
        {
            player.data.DoDamaged(damage);
            player.actor.OnUpdateHp(player.data.MaxHp, player.data.Hp);
            enemy.data.ResetAP(5);
            ++PassivePoint;
            GameTurnManager.Instance.TurnStart();
        }
        else
        {
            Debug.Log("내 턴입니다. 적이 공격할 수 없습니다.");
        }
    }
    public void HealMyActor(int addHp)
    {
        if (GameTurnManager.Instance.isMyTurn == true)
        {
            player.data.DoHeal(addHp);
            player.actor.OnUpdateHp(player.data.MaxHp, player.data.Hp);
            ++PassivePoint;
            enemy.data.MinusAP(1);
            GameTurnManager.Instance.TurnStart();
        }
        else
        {
            Debug.Log("내 턴입니다. 내가 회복 할 수 없습니다.");
        }
    }
    public void HealEnemyActor(int addHp)
    {
        if (GameTurnManager.Instance.isMyTurn == false)
        {
            enemy.data.DoHeal(addHp);
            enemy.actor.OnUpdateHp(enemy.data.MaxHp, enemy.data.Hp);
            ++PassivePoint;
            enemy.data.ResetAP(5);
            GameTurnManager.Instance.TurnStart();
        }
        else
        {
            Debug.Log("내 턴입니다. 적이 회복 할 수 없습니다.");
        }
    }

    public bool IsEnemyTurn()
    {
        // ?? 예림 : config  로 바꿀예정
        return enemy.data.GetAP() <= 0;
    }

    public bool IsDraw()
    {
        // ?? 예림 : config  로 바꿀예정
        return PassivePoint % 5 == 0;
    }
}