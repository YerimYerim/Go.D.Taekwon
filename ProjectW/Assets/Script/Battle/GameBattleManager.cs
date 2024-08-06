using System;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

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

    public event Action<int> OnEventRemoveCard;
    public event Action OnUpdateCard;

    // sourceId, spellSource
    public List<GameSpellSource> _sources = new();

    protected override void Awake()
    {
        base.Awake();
        GameDataManager.Instance.LoadData();
    }

    protected override void Init()
    {
        GameDataManager.Instance.LoadData();
        
        // spell
        spellIDs.AddRange(new []{10101,10103, 20104,20107,});
        spellDatas.Clear();
        foreach (var cardKey in spellIDs)
        {
            AddSpell(cardKey);
        }
        OnUpdateCard?.Invoke();
        

        var actorTableData = GameDataManager.Instance._actorDatas.Find(_ => _.actor_type == ACTOR_TYPE.ACTOR_TYPE_PLAYER);
        var playableTableData = GameDataManager.Instance._playableCharacterDatas.Find(_=>_.actor_id == (actorTableData?.actor_id ?? 0));

        var pcPrefab = GameUtil.GetActorPrefab(actorTableData?.rsc_id ?? 0);
        pcPrefab.transform.SetParent(GameObject.Find(GameMapManager.Instance.actorParent).transform);
        player = GameActormanager.Instance.GetActor(pcPrefab.name);
        player.data.Init(playableTableData?.stat_hp ?? 0);
        player.OnUpdateHp();
        
        
        var sourceTableData = GameDataManager.Instance._spellSourceTableDatas;

        for (int i = 0; i < sourceTableData.Count; ++i)
        {
            GameSpellSource source = new GameSpellSource();
            source.Init(sourceTableData[i]?.source_id ?? 0, MakeSpell);
            _sources.Add(source);
        }
        GameMapManager.Instance.SpawnActors();
    }

    public void UpdateEnemyHp()
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            enemy[i].OnUpdateHp();
        }
    }

    public void SetEnemyData(int i, ActorEnemyData enemyData)
    {
        enemy[i].data = enemyData;
    }

    public void SpawnEnemy(GameActor actorPrefab)
    {
        enemy.Add(GameActormanager.Instance.GetActor(actorPrefab.name));
    }
    public void RemoveAllEnemy(GameActor actorPrefab)
    {
        
    }

    public GameSpellSource GetSource(int index)
    {
        if (index >= _sources.Count)
        {
            GameDataManager.Instance.LoadData();
            var sourceTableData = GameDataManager.Instance._spellSourceTableDatas;

            for (int i = 0; i < sourceTableData.Count; ++i)
            {
                GameSpellSource source = new GameSpellSource();
                source.Init(sourceTableData[i]?.source_id ?? 0, MakeSpell);
                _sources.Add(source);
            }
        }
        return _sources[index];
    }
    private void AddSpell(int cardKey)
    {
        SpellTableData spellTableData = GameDataManager.Instance._spellData.Find(_ => _.spell_id == cardKey);
        var spellData = new GameDeckManager.SpellData(spellTableData);
        spellDatas.Add(spellData);
    }

    public int GetMyHp()
    {
        return player.data.GetHp();
    }
    public void DoSkill(int spellid, SpellEffectTableData effect)
    {
        if (Instance.IsEnemyTurn() == false)
        {
            Instance.RemoveCard(spellid);
            var skillEffectBase = GameUtil.GetSkillEffectBase(effect);
            switch (effect.target)
            {
                case TARGET_TYPE.TARGET_TYPE_SELF:
                {
                    skillEffectBase.DoSkill(new List<GameActor> {player}, player);
                } break;
                case TARGET_TYPE.TARGET_TYPE_ENEMY:
                {
                    skillEffectBase.DoSkill(enemy, player);
                } break;
            }
            ++passivePoint;
        }
        else
        {
            Debug.Log("적의 턴입니다. 공격할 수 없습니다.");
        }
    }
    /// <summary>
    ///  적이 주는 Damage
    /// </summary>
    /// <param name="damage"></param>
    public void DoSKillEnemyTurn()
    {
        if (IsEnemyTurn() == true)
        {
            for (int i = 0; i < enemy.Count; ++i)
            {
                var skill = ((ActorEnemyData)enemy[i].data).GetSkill();
                if (skill != null && ((ActorEnemyData)enemy[i].data).IsCanUseSkill())
                {
                    var skilleffect = GameDataManager.Instance._spelleffectDatas.Find(_ => _.effect_id == skill?.effect_id);
                    var skillEffectBase = GameUtil.GetSkillEffectBase(skilleffect);
                    switch (skilleffect.target)
                    {
                        case TARGET_TYPE.TARGET_TYPE_SELF:
                        {
                            skillEffectBase.DoSkill(new List<GameActor> {enemy[i]}, enemy[i]);
                        } break;
                        case TARGET_TYPE.TARGET_TYPE_ENEMY:
                        {
                            skillEffectBase.DoSkill(new List<GameActor>{player}, enemy[i]);
                        } break;
                    }
                    ((ActorEnemyData) enemy[i].data).ResetAP();
                }
            }
            player.OnUpdateHp();
            ++passivePoint;
            GameTurnManager.Instance.TurnStart();
        }
        else
        {
            Debug.Log("내 턴입니다. 적이 공격할 수 없습니다.");
        }
    }
    public void HealEnemyActor(int addHp)
    {
        if (GameTurnManager.Instance.IsMyTurn == false)
        {
            for (int i = 0; i < enemy.Count; ++i)
            {
                enemy[i].data.DoHeal(addHp);
                enemy[i].OnUpdateHp();
                ((ActorEnemyData)enemy[i].data).ResetAP();
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
        // ?? : config  로 바꿀예정
        for (int i = 0; i < enemy.Count; ++i)
        {
            if (((ActorEnemyData)enemy[i].data).IsCanUseSkill())
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
        // ?? : config  로 바꿀예정
        return passivePoint % 5 == 0;
    }

    private void RemoveCard(int id)
    {
        Instance.spellIDs.Remove(id);
        OnEventRemoveCard?.Invoke(id);
    }

    public void MinusAP(int minusAP)
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            var enemyData = (ActorEnemyData)enemy[i].data;
            enemyData.MinusAP(minusAP);
        }
        for (int i = 0; i < _sources.Count; ++i)
        {
            _sources[i].UpdateAP();
        }
    }

    private void MakeSpell(int spellID)
    {
        AddSpell(spellID);
        OnUpdateCard?.Invoke();
    }

    public bool IsAllEnemyDead()
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            if (enemy[i].data.GetHp() > 0)
            {
                return false;
            };
        }

        return true;
        
    }
}