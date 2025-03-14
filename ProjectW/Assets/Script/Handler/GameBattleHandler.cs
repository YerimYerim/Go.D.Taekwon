using System;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;

public class GameBattleHandler
{
    public readonly List<GameDeckManager.SpellData> spellDatas = new();
    public List<GameSpellSource> _sources = new();
    public UICardDeckOnHand UICardDeck;
    public UIApGauge uiApGauge;
    public List<Relic> relics = new();
    
    public event Action OnEventRemoveCard;
    public event Action OnUpdateCard;
    public event Action OnGameStart;

    // 보유한 플레이어의 레시피리스트

    public List<SpellCombineTableData> SpellCombineList = new();
    // sourceId, spellSource
    public void Init()
    {
        OnUpdateCard?.Invoke();
        spellDatas.Clear();
        SetFirstSource(GameUtil.PLAYER_ACTOR_ID);

        if (GameUIManager.Instance.TryGetOrCreate<UICardDeckOnHand>(false, UILayer.LEVEL_1, out var deckUI))
        {
            UICardDeck = deckUI;
            UICardDeck.Show();
            UICardDeck.SetUI();
        }


        if (GameUIManager.Instance.TryGetOrCreate<UIApGauge>(false, UILayer.LEVEL_1, out var gaugeUI))
        {
            uiApGauge = gaugeUI;
            uiApGauge.Show();
            uiApGauge.Init();
        }
        
        for (int i = 0; i < relics.Count; ++i)
        {
            relics[i].RegisterEvent();
        }

        OnGameStart?.Invoke();
        
        if (GameUIManager.Instance.TryGetOrCreate<UI_Popup_BattleStartEnd>(false, UILayer.LEVEL_4,
                out var uiPopupBattleStartEnd))
        {
            uiPopupBattleStartEnd.Show();
        };
    }

    public void OnDispose()
    {
        for (int i = 0; i < relics.Count; ++i)
        {
            relics[i].UnregisterEvent();
        }
        
        relics.Clear();
        _sources.Clear();
        spellDatas.Clear();
        
        UICardDeck?.Hide();
        uiApGauge?.Hide();
        
    }
    public void SetFirstSource(int actorId)
    {        
        var playableTableData = GameTableManager.Instance._playableCharacterDatas.Find(_=>_.actor_id == actorId);
        spellDatas.Clear();

        var firstreward = GameTableManager.Instance._firstRewardTable.FindAll(_ => _.first_reward_id == playableTableData.first_reward_id);
        for (int i = 0; i < firstreward.Count; ++i)
        {
            if (firstreward[i].reward_type == REWARD_TYPE.REWARD_TYPE_SPELL_SOURCE)
            {
                AddSource(firstreward[i].content_id ?? 0);
            }
            else if(firstreward[i].reward_type == REWARD_TYPE.REWARD_TYPE_SPELL_COMBINE)
            {
                AddSpellCombine(firstreward[i].content_id ?? 0);
            }
        }
    }   
    
    public void StartNewGame()
    {
        spellDatas.Clear();
    
        for(int i = 0; i< _sources.Count; ++i)
        {
            _sources[i].ResetAp();
            AddSpell(_sources[i].GetProductionSpellId(), _sources[i].GetInitProductionAmount());
        }
        OnUpdateCard?.Invoke();
        
        if (GameUIManager.Instance.TryGetOrCreate<UI_Popup_BattleStartEnd>(false, UILayer.LEVEL_4, out var uiPopupBattleStartEnd))
        {
            uiPopupBattleStartEnd.Show();
        };
    } 
    public void AddSource(int sourceId)
    {
        var sourceTableData = GameTableManager.Instance._spellSourceTableDatas.Find(_ => sourceId == _.source_id);
        _sources.Add(new GameSpellSource());
        _sources[^1].Init(sourceTableData, MakeSpell, ()=>uiApGauge.UpdateSourceUI(true));
        AddSpell(sourceTableData?.spell_id ?? 0, sourceTableData?.product_value_init ?? 0);
    }
    
    public void AddSpell(int cardKey, int amount, int selectIndex = -1)
    {
        var spellTableData = GameTableManager.Instance._spellData.Find(_ => _.spell_id == cardKey);
        var spellData = new GameDeckManager.SpellData(spellTableData);
        GameUtil.Log(GameUtil.GetString(spellData.tableData.spell_name) + amount + "생산");
        for (int i = 0; i < amount; ++i)
        {
            if(selectIndex == -1)
                spellDatas.Add(spellData);
            else
                spellDatas.Insert(selectIndex,spellData);
        }
    }
    
    public void AddSpellCombine(int contentId)
    {
        SpellCombineTableData sourceTableData = GameTableManager.Instance._spellCombineDatas.Find(_ => contentId == _.spell_combine_id);
        SpellCombineList.Add(sourceTableData);
    }
    
    public void DoSkill(GameDeckManager.SpellData spellData, GameActor targetActor)
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var handler = battleMode.PlayerActorHandler;
        var player = battleMode?.PlayerActorHandler.player;

        if (player == null)
            return;
        
        if (battleMode.ActorHandler.IsEnemyTurn() == false)
        {
            var spellEffect = spellData.tableData.spell_effect;
            bool isTargetCorrect = true;
            int spellEffectCount = spellEffect.Length;
            
            
            for (int i = 0; i < spellEffectCount; ++i)
            {
                var index = i;
                var effect = GameTableManager.Instance._spelleffectDatas.Find(_ => spellEffect[index] == _.effect_id);
                CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
                {
                    DoEffect(targetActor, effect, player, battleMode, ref isTargetCorrect);
                    player.DoAnim(SPUM_Prefabs.AnimationType.AttackMagic);
                }), 0.5f);
            }
            CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
            {
                if (isTargetCorrect)
                {
                    RemoveCard(spellData);
                    MinusResourceAP(1);
                    battleMode.ActorHandler.MinusAP(1);
                    uiApGauge.UpdateMonsterUI(true);
                }
            }),0.11f);
            CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
            {
                player.OnUpdateHp(handler.playerData);
                player.DoAnim(SPUM_Prefabs.AnimationType.Idle);
            }), 0.1f);
            CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
            {
                GameTurnManager.Instance.TurnStart();
            }), 0.1f);
            CommandManager.Instance.StartGameCommand();
        }
        else
        {
            GameUtil.Log("적의 턴입니다. 공격할 수 없습니다.");
        }
    }

    public void  DoEffect(GameActor targetActor, SpellEffectTableData effect, GameActor player, GameBattleMode battleMode, ref bool isTargetCorrect)
    {
        var skillEffectBase = SkillEffectFactory.GetSkillEffectBase(effect);
        switch (effect.target)
        {
            case TARGET_TYPE.TARGET_TYPE_SELF:
            {
                skillEffectBase.DoSkill(new List<GameActor> {player}, player);
                GameUtil.Log(player.gameObject.name + "사용" + effect.effect_type + "수치" + effect.value_1);
            }
                break;
            case TARGET_TYPE.TARGET_TYPE_ENEMY:
            {
                if (effect.target == TARGET_TYPE.TARGET_TYPE_ENEMY)
                {
                    if (targetActor != player)
                    {
                        skillEffectBase.DoSkill(new List<GameActor> {targetActor}, player);
                        GameUtil.Log(targetActor.gameObject.name + "에게 사용" + (effect?.effect_type ?? null) + "수치" + (effect?.value_1 ?? 0));
                    }
                    else
                    {
                        isTargetCorrect = false;
                        GameUtil.Log("대상이 잘못지정되었습니다. ");
                    }
                }
            }
                break;
            case TARGET_TYPE.TARGET_TYPE_ENEMY_ALL:
            {
                if (targetActor != player)
                {
                    skillEffectBase.DoSkill(battleMode.ActorHandler.GetEnemyData(), player);
                    GameUtil.Log("모든 적에게 사용" + (effect?.effect_type ?? null) + "수치" +
                                 (effect?.value_1 ?? 0));
                }
                else
                {
                    isTargetCorrect = false;
                    GameUtil.Log("대상이 잘못지정되었습니다. ");
                }
            }
                break;
        }
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
        var ActorHandler = battleMode.ActorHandler;
        
        var player = handler?.player;
        if (player == null)
            return;
        
        if(ActorHandler == null)
            return;

        if (battleMode.ActorHandler.IsEnemyTurn() == true)
        {
            for (int i = 0; i <  ActorHandler.GetEnemyCount(); ++i)
            {
                var enemyData = ActorHandler.GetEnemyData(i);
                var enemyActor = ActorHandler.GetEnemy(i);
                var enemyActorAll = ActorHandler.GetEnemyAll();
                var skills = enemyData.GetSkillID();
                if (enemyData.IsCanUseSkill())
                {
                    foreach(var skill in skills)
                    {
                        var skillEffect = GameTableManager.Instance._spelleffectDatas.Find(_ => _.effect_id == skill);
                        var skillEffectBase = SkillEffectFactory.GetSkillEffectBase(skillEffect);
                        switch (skillEffect.target)
                        {
                            case TARGET_TYPE.TARGET_TYPE_SELF:
                            {
                                EnemyTurnCommand enemyTurnCommand = new EnemyTurnCommand(() =>
                                {
                                    skillEffectBase.DoSkill(enemyActorAll, enemyActor);
                                    enemyData.ResetAP();
                                    player.OnUpdateHp(handler.playerData);
                                    uiApGauge.UpdateMonsterUI(true);
                                });
                                CommandManager.Instance.AddCommand(enemyTurnCommand, 0.1f);
                                GameUtil.Log(enemyActor.gameObject.name + "사용" + skillEffect.effect_type + "수치" +
                                             skillEffect.value_1);
                            }
                            break;
                            case TARGET_TYPE.TARGET_TYPE_ALLY_ALL:
                            {
                                EnemyTurnCommand enemyTurnCommand = new EnemyTurnCommand(() =>
                                {
                                    skillEffectBase.DoSkill(new List<GameActor> {enemyActor}, enemyActor);
                                    enemyData.ResetAP();
                                    player.OnUpdateHp(handler.playerData);
                                    uiApGauge.UpdateMonsterUI(true);
                                });
                                CommandManager.Instance.AddCommand(enemyTurnCommand, 0.1f);
                                GameUtil.Log(enemyActor.gameObject.name + "사용" + skillEffect.effect_type + "수치" +
                                             skillEffect.value_1);
                            } break;
                            case TARGET_TYPE.TARGET_TYPE_ENEMY:
                            {
                                EnemyTurnCommand enemyTurnCommand = new EnemyTurnCommand(() =>
                                {
                                    skillEffectBase.DoSkill(new List<GameActor> {player}, enemyActor);
                                    enemyData.ResetAP();
                                    player.OnUpdateHp(player.data);
                                    uiApGauge.UpdateMonsterUI(true);
                                });
                                CommandManager.Instance.AddCommand(enemyTurnCommand, 0.1f);
                                GameUtil.Log(enemyActor.gameObject.name + "사용" + skillEffect.effect_type + "수치" +
                                             skillEffect.value_1);

                            }
                                break;
                        }
                    }
                   
                }
            }
            CommandManager.Instance.AddCommand(new EnemyTurnCommand(GameTurnManager.Instance.TurnStart),0.5f);
            CommandManager.Instance.StartGameCommand();
        }
        else
        {
            GameUtil.Log("내 턴입니다. 적이 공격할 수 없습니다.");
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

        if (battleMode.ActorHandler.GetEnemyCount() > 0)
        {
            battleMode.ActorHandler.UpdateEnemyHp();
            battleMode.ActorHandler.UpdateTurnSkill();
        }
        
        player.OnUpdateHp(handler.playerData);
        player.UpdateTurnSkill();
        
        if(spellDatas.Count == 0)
        {
            CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
            {
                MinusResourceAP(1);
                battleMode.ActorHandler.MinusAP(1);
                uiApGauge.UpdateMonsterUI(true);
                player.OnUpdateHp(handler.playerData);
                player.UpdateTurnSkill();
                GameUtil.Log("spellDatas.Count == 0");
            }),0.2f);
            //CommandManager.Instance.StartGameCommand();
        }
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
            _sources[i].ReduceAp(minusAP);
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