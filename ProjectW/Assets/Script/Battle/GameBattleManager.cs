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
    public List<GameActor> enemy = new();
    public GameActor player = new();
    
    public int PassivePoint = 1;
    public List<GameDeckManager.SpellData> spellDatas = new();
    public List<int> spellIDs = new();

    public event Action<int> onEventAction;
    public void Init()
    {
        enemy.Add(GameActormanager.Instance.GetActor("ActorEnemy"));
        for(int i = 0; i< enemy.Count; ++i)
        {
            enemy[i].data.Init(10, 5);
            enemy[i].OnUpdateHp();
        }
        player = GameActormanager.Instance.GetActor("ActorPlayer");
        player.data.Init(10);
        player.OnUpdateHp();
    }

    public int GetMyHp()
    {
        return player.data.GetHp();
    }
    public void DoSkill(int spellid, SpellEffectTableData effect, List<GameActor> targetActor)
    {
        if (GameBattleManager.Instance.IsEnemyTurn() == false)
        {
            GameBattleManager.Instance.RemoveCard(spellid);
            var skillEffectBase = GameUtil.GetSkillEffectBase(effect);
            skillEffectBase.DoSkill(targetActor, player);
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
            for (int i = 0; i < enemy.Count; ++i)
            {
                enemy[i].data.ResetAP(5);
            }
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
            for (int i = 0; i < enemy.Count; ++i)
            {
                enemy[i].data.MinusAP(1);
            }
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
            for (int i = 0; i < enemy.Count; ++i)
            {
                enemy[i].data.DoHeal(addHp);
                enemy[i].OnUpdateHp();
                enemy[i].data.ResetAP(5);
            }
            ++PassivePoint;
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
        for (int i = 0; i < enemy.Count; ++i)
        {
            if (enemy[i].data.GetAP() <= 0)
            {
                return true;
            }
        }

        return false;
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