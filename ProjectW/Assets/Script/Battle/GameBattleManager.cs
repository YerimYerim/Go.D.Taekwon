using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Script.Manager;
using UnityEngine;

/// <summary>
/// battlemanager
/// </summary>
public class GameBattleManager : Singleton<GameBattleManager>
{
    public GameActor enemy = new();
    public GameActor player = new();
    
    public int PassivePoint = 1;
    public List<GameDeckManager.SpellData> spellDatas = new();
    public List<int> spellIDs = new();

    public event Action<int> onEventAction;
    public void Init()
    {
        enemy = GameActormanager.Instance.GetActor("ActorEnemy");
        enemy.data.Init(10, 5);
        enemy.OnUpdateHp();
        player = GameActormanager.Instance.GetActor("ActorPlayer");
        player.data.Init(10);
        player.OnUpdateHp();
    }

    public int GetEnemyHp()
    {
        return enemy.data.GetHp();
    }

    public int GetMyHp()
    {
        return player.data.GetHp();
    }
    public void Attack(int spellid, SpellEffectTableData effect, GameActor targetActor)
    {
        if (GameBattleManager.Instance.IsEnemyTurn() == false)
        {
            GameBattleManager.Instance.RemoveCard(spellid);
            var skillEffectBase = GameUtil.GetSkillEffectBase(effect);
            skillEffectBase.DoSkill();
            
            targetActor.data.DoDamaged(skillEffectBase.GetDamage());
            targetActor.data.MinusAP(1);
            targetActor.OnUpdateHp();

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
            player.OnUpdateHp();
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
            player.OnUpdateHp();
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
            enemy.OnUpdateHp();
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

    public void RemoveCard(int id)
    {
        GameBattleManager.Instance.spellIDs.Remove(id);
        onEventAction?.Invoke(id);
    }
}