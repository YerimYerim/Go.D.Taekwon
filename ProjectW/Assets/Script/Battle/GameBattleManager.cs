using Script.Manager;
using UnityEngine;

/// <summary>
/// battlemanager
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    private ActorDataBase _enemy = new ActorDataBase();
    private ActorDataBase _my = new ActorDataBase();
    public int PassivePoint = 1;
    public void Init()
    {
        _enemy.Init(10, 5);
        _my.Init(10);
    }

    public int GetEnemyHp()
    {
        return _enemy.GetHp();
    }

    public int GetMyHp()
    {
        return _my.GetHp();
    }
    public void Attack(int damage)
    {
        if (GameTurnManager.Instance.isMyTurn == true)
        {
            _enemy.DoDamaged(damage);
            _enemy.MinusAP(1);
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
            _my.DoDamaged(damage);
            _enemy.ResetAP(5);
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
            _my.DoHeal(addHp);
            ++PassivePoint;
            _enemy.MinusAP(1);
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
            _enemy.DoHeal(addHp);
            ++PassivePoint;
            _enemy.ResetAP(5);
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
        return _enemy.GetAP() <= 0;
    }

    public bool IsDraw()
    {
        // ?? 예림 : config  로 바꿀예정
        return PassivePoint % 5 == 0;
    }
}