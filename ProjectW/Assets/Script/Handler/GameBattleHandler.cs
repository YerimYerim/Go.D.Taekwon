using System;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class GameBattleHandler
{
    public readonly List<GameDeckManager.SpellData> spellDatas = new();
    public List<GameSpellSource> _sources = new();
    public UICardDeckOnHand UICardDeck;
    public UIApGauge uiApGauge;
    public event Action OnEventRemoveCard;
    public event Action OnUpdateCard;

    // sourceId, spellSource
    public void Init()
    {
        OnUpdateCard?.Invoke();
        spellDatas.Clear();
        SetFirstSource(GameUtil.PLAYER_ACTOR_ID);

        if(GameUIManager.Instance.TryGetOrCreate<UICardDeckOnHand>(false, UILayer.LEVEL_1, out var deckUI))
        {
            UICardDeck = deckUI;
            UICardDeck.Show();
            UICardDeck.SetUI();
        }
        
        
        if(GameUIManager.Instance.TryGetOrCreate<UIApGauge>(false, UILayer.LEVEL_1, out var gaugeUI))
        {
            uiApGauge = gaugeUI;
            uiApGauge.Show();
            uiApGauge.Init();
        }
    }

    public void SetFirstSource(int actorId)
    {        
        var playableTableData = GameDataManager.Instance._playableCharacterDatas.Find(_=>_.actor_id == actorId);
        spellDatas.Clear();
        _sources.Clear();
        
        var firstreward = GameDataManager.Instance._firstRewardTable.FindAll(_ => _.first_reward_id == playableTableData.first_reward_id);
        for (int i = 0; i < firstreward.Count; ++i)
        {
            var sourceTableData = GameDataManager.Instance._spellSourceTableDatas.Find(_ => firstreward[i].source_id == _.source_id);
            _sources.Add(new GameSpellSource());
            _sources[i].Init(sourceTableData , MakeSpell, ()=>uiApGauge.UpdateSourceUI(true));
            AddSpell(sourceTableData?.spell_id ?? 0,  sourceTableData?.product_value_init ?? 0);
        }
    }
    
    public void AddSpell(int cardKey, int amount)
    {
        SpellTableData spellTableData = GameDataManager.Instance._spellData.Find(_ => _.spell_id == cardKey);
        var spellData = new GameDeckManager.SpellData(spellTableData);
        Debug.Log(GameUtil.GetString(spellData.tableData.spell_name) + amount + "생산");
        for (int i = 0; i < amount; ++i)
        {
            spellDatas.Add(spellData);
        }
    }

    public void DoSkill(GameDeckManager.SpellData spellData, GameActor targetActor)
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var handler = battleMode.PlayerActorHandler;
        var player = battleMode?.PlayerActorHandler.player;

        if (player == null)
            return;
        
        if (battleMode.ActorSpawner.IsEnemyTurn() == false)
        {
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
                            skillEffectBase.DoSkill(new List<GameActor> {battleMode.PlayerActorHandler.player}, player);
                            Debug.Log(player.gameObject.name +"사용"+ effect.effect_type + "수치" + effect.value_1);
                        } break;
                        case TARGET_TYPE.TARGET_TYPE_ENEMY:
                        {
                            if (effect.target == TARGET_TYPE.TARGET_TYPE_ENEMY)
                            {
                                if (targetActor != player)
                                {
                                    skillEffectBase.DoSkill(new List<GameActor> {targetActor}, player);
                                    Debug.Log(targetActor.gameObject.name + "에게 사용" + (effect?.effect_type ?? null) + "수치" + (effect?.value_1 ?? 0));
                                }
                                else
                                {
                                    Debug.Log("대상이 잘못지정되었습니다. ");
                                    return;
                                }
                            }
                            
                        } break;
                    }
                }
                
                RemoveCard(spellData);
                MinusResourceAP(1);
                battleMode.ActorSpawner.MinusAP(1);
                uiApGauge.UpdateMonsterUI(true);
            }), 0.11f);
            CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
            {
                player.OnUpdateHp(handler.playerData);
            }), 0.1f);
            CommandManager.Instance.StartGameCommand();
        }
        else
        {
            Debug.Log("적의 턴입니다. 공격할 수 없습니다.");
        }

        Debug.Log(player.gameObject.name + "의 체력: " + handler.playerData.Hp + "방어도:" + handler.playerData.GetAmor());
    }

    public void UpdateUIApGauge()
    {
        uiApGauge.UpdateUI();
    }
    /// <summary>
    ///  적이 주는 Damage
    /// </summary>
    /// <param name="damage"></param>
    public void DoSkillEnemyTurn()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var handler = battleMode.PlayerActorHandler;
        var ActorHandler = battleMode.ActorSpawner;
        
        var player = handler?.player;
        if (player == null)
            return;
        
        if(ActorHandler == null)
            return;

        if (battleMode.ActorSpawner.IsEnemyTurn() == true)
        {
            for (int i = 0; i <  ActorHandler.GetEnemyCount(); ++i)
            {
                var enemyData = ActorHandler.GetEnemyData(i);
                var enemyActor = ActorHandler.GetEnemy(i);
                var skill = enemyData.GetSkillID();
                if (enemyData.IsCanUseSkill())
                {
                    var skilleffect = GameDataManager.Instance._spelleffectDatas.Find(_ => _.effect_id == skill);
                    var skillEffectBase = GameUtil.GetSkillEffectBase(skilleffect);
                    switch (skilleffect.target)
                    {
                        case TARGET_TYPE.TARGET_TYPE_SELF:
                        {
                            EnemyTurnCommand enemyTurnCommand = new EnemyTurnCommand(() =>
                            {
                                skillEffectBase.DoSkill(new List<GameActor> {enemyActor}, enemyActor);
                                enemyData.ResetAP();
                                player.OnUpdateHp(handler.playerData);
                                uiApGauge.UpdateMonsterUI(true);
                            });
                            CommandManager.Instance.AddCommand(enemyTurnCommand,0.5f);
                            Debug.Log(enemyActor.gameObject.name +"사용"+ skilleffect.effect_type + "수치" + skilleffect.value_1);
                        
                        } break;
                        case TARGET_TYPE.TARGET_TYPE_ENEMY:
                        {
                            EnemyTurnCommand enemyTurnCommand = new EnemyTurnCommand(() =>
                            {
                                skillEffectBase.DoSkill(new List<GameActor>{player}, enemyActor);
                                enemyData.ResetAP();
                                player.OnUpdateHp(handler.playerData);
                                uiApGauge.UpdateMonsterUI(true);
                            });
                            CommandManager.Instance.AddCommand(enemyTurnCommand,0.5f);
                            Debug.Log(enemyActor.gameObject.name +"사용"+ skilleffect.effect_type + "수치" + skilleffect.value_1);
                            
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
    

    public void DoTurn()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var handler = battleMode.PlayerActorHandler;
        var player = handler?.player;
        
        if (player == null)
        {
            return;
        }
        battleMode.ActorSpawner.UpdateEnemyHp();
        battleMode.ActorSpawner.UpdateTurnSkill();
        
        player.OnUpdateHp(handler.playerData);
        player.UpdateTurnSkill();

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

    public void MinusResourceAP(int minusAP)
    {
        for (int i = 0; i < _sources.Count; ++i)    
        {
            _sources[i].ReduceAP(minusAP);
        }
        uiApGauge.UpdateSourceUI(true);
        DoTurn();
    }

    private void MakeSpell(int spellID, int amount)
    {
        AddSpell(spellID, amount);
        OnUpdateCard?.Invoke();
    }
}