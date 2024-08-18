using System;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class GameBattleManager : Singleton<GameBattleManager>
{
    public List<GameActor> enemy = new();
    public GameActor player = new();
    
    public readonly List<GameDeckManager.SpellData> spellDatas = new();
    public List<int> spellIDs = new();
    public List<GameSpellSource> _sources = new();
    public event Action OnEventRemoveCard;
    public event Action OnUpdateCard;

    // sourceId, spellSource

    protected override void Awake()
    {
        base.Awake();
        GameDataManager.Instance.LoadData();
    }

    protected override void Init()
    {
        GameDataManager.Instance.LoadData();
        

        OnUpdateCard?.Invoke();
        

        var actorTableData = GameDataManager.Instance._actorDatas.Find(_ => _.actor_type == ACTOR_TYPE.ACTOR_TYPE_PLAYER);
        var playableTableData = GameDataManager.Instance._playableCharacterDatas.Find(_=>_.actor_id == (actorTableData?.actor_id ?? 0));
        var firstreward = GameDataManager.Instance._firstRewardTable.FindAll(_ => _.first_reward_id == playableTableData.first_reward_id);
        
        var pcPrefab = GameUtil.GetActorPrefab(actorTableData?.rsc_id ?? 0);
        pcPrefab.transform.SetParent(GameObject.Find(GameMapManager.Instance.actor).transform);
        GameActormanager.Instance.AddActors(pcPrefab.name, pcPrefab);
        player = GameActormanager.Instance.GetActor(pcPrefab.name);
        var playerData =  new ActorPlayerData();
        playerData.Init(playableTableData?.stat_hp ?? 0);
        player.data = playerData;
        player.OnUpdateHp();


        spellDatas.Clear();

        for (int i = 0; i < firstreward.Count; ++i)
        {
            var sourceTableData = GameDataManager.Instance._spellSourceTableDatas.Find(_=>firstreward[i].source_id == _.source_id);
            GameSpellSource source = new GameSpellSource();
            source.Init(sourceTableData?.source_id ?? 0, MakeSpell);
            _sources.Add(source);
            
            AddSpell(sourceTableData?.spell_id ?? 0, sourceTableData?.product_value_init ??0);
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
    public void RemoveAllEnemy()
    {
        enemy.Clear();
    }

    public GameSpellSource GetSource(int index)
    {
        if (index >=  _sources.Count)
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
    public void AddSpell(int cardKey, int amount)
    {
        SpellTableData spellTableData = GameDataManager.Instance._spellData.Find(_ => _.spell_id == cardKey);
        var spellData = new GameDeckManager.SpellData(spellTableData);
        for (int i = 0; i < amount; ++i)
        {
            Debug.Log(GameUtil.GetString(spellData.tableData.spell_name) + amount + "생산");
            spellDatas.Add(spellData);
        }
    }

    public int GetMyHp()
    {
        return player.data.GetHp();
    }
    public void DoSkill(GameDeckManager.SpellData spellData, GameActor targetActor)
    {
        if (Instance.IsEnemyTurn() == false)
        {
            Instance.RemoveCard(spellData);
            CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
            {
                var spellEffect = spellData.tableData.spell_effect;
                for (int i = 0; i < spellEffect.Length; ++i)
                {
                    var index = i;
                    var effect = GameDataManager.Instance._spelleffectDatas.Find(_ => spellEffect[index] == _.effect_id);
                    var skillEffectBase = GameUtil.GetSkillEffectBase(effect);
                    switch (effect.target)
                    {
                        case TARGET_TYPE.TARGET_TYPE_SELF:
                        {
                            skillEffectBase.DoSkill(new List<GameActor> {player}, player);

                        }
                            break;
                        case TARGET_TYPE.TARGET_TYPE_ENEMY:
                        {
                            if(effect.target == TARGET_TYPE.TARGET_TYPE_ENEMY)
                                skillEffectBase.DoSkill(new List<GameActor> {targetActor}, player);
                            else if(effect.target == TARGET_TYPE.TARGET_TYPE_ENEMY)
                                skillEffectBase.DoSkill(enemy, player);
                        }
                            break;
                    }
                }
                MinusAP(1);
                GameTurnManager.Instance.TurnStart();
            }), 0.1f);

            CommandManager.Instance.StartGameCommand();
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
    public void DoSkillEnemyTurn()
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
                    var index = i;
                    switch (skilleffect.target)
                    {
                        case TARGET_TYPE.TARGET_TYPE_SELF:
                        {
                            EnemyTurnCommand enemyTurnCommand = new EnemyTurnCommand(() =>
                            {
                                skillEffectBase.DoSkill(new List<GameActor> {enemy[index]}, enemy[index]);
                                ((ActorEnemyData) enemy[index].data).ResetAP();
                            });
                            CommandManager.Instance.AddCommand(enemyTurnCommand,0.5f);
                        
                        } break;
                        case TARGET_TYPE.TARGET_TYPE_ENEMY:
                        {
                            EnemyTurnCommand enemyTurnCommand = new EnemyTurnCommand(() =>
                            {
                                skillEffectBase.DoSkill(new List<GameActor>{player}, enemy[index]);
                                ((ActorEnemyData) enemy[index].data).ResetAP();
                                player.OnUpdateHp();
                            });
                            CommandManager.Instance.AddCommand(enemyTurnCommand,0.5f);
                            
                        } break;
                    }
                   
                }
            }
            CommandManager.Instance.AddCommand(new EnemyTurnCommand(GameTurnManager.Instance.TurnStart),0.5f);
            CommandManager.Instance.StartGameCommand();
        }
        else
        {
            Debug.Log("내 턴입니다. 적이 공격할 수 없습니다.");
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
        return _sources.Any(t => t.GetRemainAp() <= 0);
    }

    private void RemoveCard(GameDeckManager.SpellData spell)
    {
        spellDatas.Remove(spell);
        OnEventRemoveCard?.Invoke();
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

    private void MakeSpell(int spellID, int amount)
    {
        AddSpell(spellID, amount);
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