using System;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class GameBattleHandler
{
    private List<GameActor> enemy = new();

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
        var playableTableData = GameDataManager.Instance._playableCharacterDatas.Find(_=>_.actor_id == GameUtil.PLAYER_ACTOR_ID);
        var firstreward = GameDataManager.Instance._firstRewardTable.FindAll(_ => _.first_reward_id == playableTableData.first_reward_id);
        for (int i = 0; i < firstreward.Count; ++i)
        {
            var sourceTableData = GameDataManager.Instance._spellSourceTableDatas.Find(_=>firstreward[i].source_id == _.source_id);
            _sources.Add(new GameSpellSource());
            _sources[i].Init(sourceTableData?.source_id ?? 0, MakeSpell);
            AddSpell(sourceTableData?.spell_id ?? 0, sourceTableData?.product_value_init ??0);
        }
        GameMapManager.Instance.SpawnActors();
        
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

    public void UpdateEnemyHp()
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            enemy[i].OnUpdateHp(enemy[i].data);
        }
    }

    public void SetEnemyData(int i, ActorEnemyData enemyData)
    {
        enemy[i].data = enemyData;
    }

    public void SpawnEnemy(GameActor actorPrefab)
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var actorSpawner = gameBattleMode?.BattleActorSpawner;
        if(actorSpawner == null)
            return;
        enemy.Add(actorSpawner.GetActor(actorPrefab.name));
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
        
        if (IsEnemyTurn() == false)
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
                MinusAP(1);
                player.OnUpdateHp(handler.playerData);
            }), 1f);

            CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
            {
                GameTurnManager.Instance.TurnStart();
            }), 0);
            CommandManager.Instance.StartGameCommand();
        }
        else
        {
            Debug.Log("적의 턴입니다. 공격할 수 없습니다.");
        }

        Debug.Log(player.gameObject.name + "의 체력: " + handler.playerData.Hp + "방어도:" + handler.playerData.GetAmor());
    }
    /// <summary>
    ///  적이 주는 Damage
    /// </summary>
    /// <param name="damage"></param>
    public void DoSkillEnemyTurn()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var handler = battleMode.PlayerActorHandler;
        var player = handler?.player;
        if (player == null)
            return;
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
                            Debug.Log(enemy[i].gameObject.name +"사용"+ skilleffect.effect_type + "수치" + skilleffect.value_1);
                        
                        } break;
                        case TARGET_TYPE.TARGET_TYPE_ENEMY:
                        {
                            EnemyTurnCommand enemyTurnCommand = new EnemyTurnCommand(() =>
                            {
                                skillEffectBase.DoSkill(new List<GameActor>{player}, enemy[index]);
                                ((ActorEnemyData) enemy[index].data).ResetAP();
                                player.OnUpdateHp(handler.playerData);
                            });
                            CommandManager.Instance.AddCommand(enemyTurnCommand,0.5f);
                            Debug.Log(enemy[i].gameObject.name +"사용"+ skilleffect.effect_type + "수치" + skilleffect.value_1);
                            
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
            enemy[i].OnUpdateHp(enemy[i].data);
        }
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var handler = battleMode.PlayerActorHandler;
        var player = handler?.player;
        
        if (player == null)
        {
            return;
        }
        player.UpdateDebuff();
        player.UpdateBuff();
        player.OnUpdateHp(handler.playerData);
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
            _sources[i].ReduceAP(minusAP);
        }

        uiApGauge.UpdateUI();
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