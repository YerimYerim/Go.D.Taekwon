using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Relic
{
    private record Condition 
    {
        public ACTIVE_CONDITION condition;
        public int ConditionValue;
        public bool IsActivated;
        public bool IsCheck;
    }
    
    private readonly RelicTableData _relicTableData;
    
    private readonly List<Condition> _conditions = new();
    private readonly List<SpellEffectTableData> _effects = new();
    private Action<int> _onEventDamagedAction;
    private LOGICAL_OPERATOR ConditionLogic => _relicTableData.condition_logic ?? LOGICAL_OPERATOR.LOGICAL_OPERATOR_AND;
    
    private bool IsAllConditionActivated
    {
        get
        {
            if (ConditionLogic == LOGICAL_OPERATOR.LOGICAL_OPERATOR_AND)
            {
                return _conditions.All(_ => _.IsActivated);
            }
            else
            {
                return _conditions.Any(_ => _.IsActivated);
            }
        }
    }

    private int conditionCount
    {
        get {
            int count = 0;
            if(_relicTableData.active_condition_1 != null)
                ++count;
            if (_relicTableData.active_condition_2 != null)
                ++count;
            return count;
        }
    }
    
    private int effectCount => _relicTableData.relic_effect.Length;

    public Relic(RelicTableData relicTableData)
    {
        this._relicTableData = relicTableData;
        for(int i  = 0; i< conditionCount; ++i)
        {
            switch (i)
            {
                case 0:
                {
                    var condition = new Condition
                    {
                        condition = relicTableData.active_condition_1 ?? ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START,
                        ConditionValue = relicTableData.active_value_1 ?? 0,
                        IsActivated = false,
                    };
                    _conditions.Add(condition);
                    break;
                }
                case 1:
                {
                    var condition = new Condition
                    {
                        condition = relicTableData.active_condition_2 ?? ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START,
                        ConditionValue = relicTableData.active_value_2 ?? 0,
                        IsActivated = false,
                    };
                    _conditions.Add(condition);
                    break;
                }
            }
        }
        for(int j = 0; j < relicTableData.relic_effect.Length; ++j)
            _effects.Add(GameTableManager.Instance._spelleffectDatas.Find(_=>_.effect_id == relicTableData.relic_effect[j]));

    }

    private Action GetActiveAction(ACTIVE_CONDITION? activeCondition)
    {
        return activeCondition switch
        {
            ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START => OnBattleStart,
            ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_END => OnBattleEnd,
            ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT => OnHPPercentMinus,
            ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE => OnHPMinus,
            _ => null
        };
    }

    void RegisterCondition(ACTIVE_CONDITION? condition, Action action)
    {
         var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        switch (condition)
        {
            case ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START:
                gameMode.BattleHandler.OnGameStart += action;
                break;
            case ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_END:
                gameMode.MapHandler.OnGameEnd += action;
                break;
            case ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT:
            case ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE:
                gameMode.PlayerActorHandler.player.data.OnEventDamaged += _onEventDamagedAction = (int _) =>
                {
                    action?.Invoke();
                };
                break;
        }
    }
    public void RegisterEvent()
    {
        if (ConditionLogic == LOGICAL_OPERATOR.LOGICAL_OPERATOR_AND)
        {
            RegisterCondition(_relicTableData.active_condition_1, GetActiveAction(_relicTableData.active_condition_1));
            RegisterCondition(_relicTableData.active_condition_1, GetActiveAction(_relicTableData.active_condition_2));
        }
        else if (ConditionLogic == LOGICAL_OPERATOR.LOGICAL_OPERATOR_OR)
        {
            RegisterCondition(_relicTableData.active_condition_1, GetActiveAction(_relicTableData.active_condition_1));
            RegisterCondition(_relicTableData.active_condition_2, GetActiveAction(_relicTableData.active_condition_2));
        }
    }
    
    private void UnregisterCondition(ACTIVE_CONDITION? condition, Action action)
    {
        var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        switch (condition)
        {
            case ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START:
                gameMode.BattleHandler.OnGameStart -= action;
                break;
            case ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_END:
                gameMode.MapHandler.OnGameEnd -= action;
                break;
            case ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT:
            case ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE:
                gameMode.PlayerActorHandler.player.data.OnEventDamaged -= _onEventDamagedAction = (int _) =>
                {
                    action?.Invoke();
                };
                break;
        }
    }
        
    public void UnregisterEvent()
    {
        if (ConditionLogic == LOGICAL_OPERATOR.LOGICAL_OPERATOR_AND)
        {
            UnregisterCondition(_relicTableData.active_condition_1, GetActiveAction(_relicTableData.active_condition_1));
            UnregisterCondition(_relicTableData.active_condition_1, GetActiveAction(_relicTableData.active_condition_2));
        }
        else if (ConditionLogic == LOGICAL_OPERATOR.LOGICAL_OPERATOR_OR)
        {
            UnregisterCondition(_relicTableData.active_condition_1, GetActiveAction(_relicTableData.active_condition_1));
            UnregisterCondition(_relicTableData.active_condition_2, GetActiveAction(_relicTableData.active_condition_2));
        }
    }
    
   
    private void OnBattleStart()
    {
        ActivateCondition(ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START, true);
    }

    private void OnBattleEnd()
    {
        ActivateCondition(ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_END, true);
    }

    private void OnHPMinus()
    {
        bool isActivated = false;
        
        var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var condition = _conditions.Find(_ => _.condition == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE);
        if (gameMode.PlayerActorHandler.player.data.GetHp() <= condition.ConditionValue)
        {
            isActivated = true;
        }
        
        ActivateCondition(ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE, isActivated);
    }

    private void OnHPPercentMinus()
    {
        bool isActivated = false;
        var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var hpPercent = (float)gameMode.PlayerActorHandler.player.data.GetHp()/  gameMode.PlayerActorHandler.player.data.GetMaxHp();
        hpPercent *= 100;
        var condition = _conditions.Find(_ => _.condition == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT);
        if (hpPercent <= condition?.ConditionValue)
        {
            isActivated = true;
        }
        ActivateCondition(ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT, isActivated);
    }

    private void ActivateCondition(ACTIVE_CONDITION conditionType, bool isActivated)
    {
        var index = _conditions.FindIndex(_ => _.condition == conditionType);
        if (_conditions[index] == null)
            return;
        
        _conditions[index].IsActivated = isActivated;
        _conditions[index].IsCheck = true;
        
        if (IsAllConditionActivated)
        {
            for (int i = 0; i < effectCount; ++i)
            {
                DoEffect(_effects[i]);
            }
        }

        switch (ConditionLogic)
        {
            case LOGICAL_OPERATOR.LOGICAL_OPERATOR_OR:
                _conditions.ForEach(_ => { _.IsActivated = false; _.IsCheck = false; });
                break;
            case LOGICAL_OPERATOR.LOGICAL_OPERATOR_AND:
            {
                if (_conditions.All(_ => _.IsCheck))
                {
                    _conditions.ForEach(_ => { _.IsActivated = false; _.IsCheck = false; });
                }
                break;
            }
        }
    }
    
    private void DoEffect(SpellEffectTableData effect)
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var handler = battleMode.PlayerActorHandler;
        var player = battleMode?.PlayerActorHandler.player;

        var enemy =  battleMode.ActorHandler.GetEnemyData();
        if (player == null)
            return;
        bool isTargetCorrect = true;
        CommandManager.Instance.AddCommand(
            new PlayerTurnCommand(() => { battleMode.BattleHandler.DoEffect(enemy[Random.Range(0, enemy.Count)], effect, player, battleMode, ref isTargetCorrect); }),
            0.1f);
        
        CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
        {
            player.OnUpdateHp(handler.playerData);
        }), 0.1f);
        CommandManager.Instance.StartGameCommand();
    }
    

    public string GetImage()
    {
        return _relicTableData.relic_resource;
    }
    
    public string GetDesc()
    {
        return GameUtil.GetString(_relicTableData.relic_desc);
    }

    public string GetName()
    {
        return GameUtil.GetString(_relicTableData.relic_name);
    }
}
