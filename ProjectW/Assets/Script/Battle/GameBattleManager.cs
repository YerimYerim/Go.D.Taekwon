using System;
using System.Collections.Generic;
using Script.Manager;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// battlemanager
/// </summary>
public class GameBattleManager : Singleton<GameBattleManager>
{
    public List<GameActor> enemy = new();
    public GameActor player = new();
    
    public int passivePoint = 1;
    public readonly List<GameDeckManager.SpellData> spellDatas = new();
    public List<int> spellIDs = new();

    public string actorParent = "Actors";
    public event Action<int> OnEventRemoveCard;

    protected override void Init()
    {
        // 스테이지에서 몬스터 정보 로드 후 프리팹 생성
        // 아직 스테이지 관련된게 없어서 걍 액터 id 에 있는거 주워옴
        var actorTableData = GameDataManager.Instance._actorDatas.Find(_ => _.actor_type == ACTOR_TYPE.ACTOR_TYPE_PLAYER);
        var playableTableData = GameDataManager.Instance._playableCharacterDatas.Find(_=>_.actor_id == (actorTableData?.actor_id ?? 0));
        
        var actorMonsterDatas = GameDataManager.Instance._actorDatas.FindAll(_ => _.actor_type == ACTOR_TYPE.ACTOR_TYPE_MONSTER);

        var pcPrefab = GameUtil.GetActorPrefab(actorTableData?.rsc_id ?? 0);
        pcPrefab.transform.SetParent(GameObject.Find(actorParent).transform);
        player = GameActormanager.Instance.GetActor(pcPrefab.name);
        player.data.Init(playableTableData?.stat_hp ?? 0);
        player.OnUpdateHp();
        
        for (int i = 0; i < actorMonsterDatas.Count; ++i)
        {
            var actorPrefab = GameUtil.GetActorPrefab(actorMonsterDatas[i]?.rsc_id ?? 0);
            actorPrefab.transform.SetParent(GameObject.Find(actorParent).transform);
            var monsterData = GameDataManager.Instance._monsterTableDatas.Find(_ => _.actor_id == actorMonsterDatas[i].actor_id);
            enemy.Add(GameActormanager.Instance.GetActor(actorPrefab.name));

            var enemyData = new ActorEnemyData();
            enemyData.Init(monsterData?.stat_hp?? 0);
            enemyData.InitAP(10);
            
            enemy[i].data = enemyData;
            enemy[i].OnUpdateHp();
        }
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
            ++passivePoint;
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
                ((ActorEnemyData)enemy[i].data).ResetAP(5);
            }
            ++passivePoint;
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
            ++passivePoint;
            for (int i = 0; i < enemy.Count; ++i)
            {
                ((ActorEnemyData)enemy[i].data).MinusAP(1);
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
                ((ActorEnemyData)enemy[i].data).ResetAP(5);
            }
            ++passivePoint;
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
            if (((ActorEnemyData)enemy[i].data).GetAP() <= 0)
            {
                return true;
            }
        }

        return false;
    }

    public void DoTurn()
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            enemy[i].UpdateDebuff();
            enemy[i].UpdateBuff();
            enemy[i].OnUpdateHp();
        }
        player.UpdateDebuff();
        player.UpdateBuff();
        
        player.OnUpdateHp();
    }
    public bool IsDraw()
    {
        // ?? 예림 : config  로 바꿀예정
        return passivePoint % 5 == 0;
    }

    public void RemoveCard(int id)
    {
        GameBattleManager.Instance.spellIDs.Remove(id);
        OnEventRemoveCard?.Invoke(id);
    }
}