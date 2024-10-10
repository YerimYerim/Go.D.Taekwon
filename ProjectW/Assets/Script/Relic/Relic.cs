using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Relic
{
    private record Condition 
    {
        public ACTIVE_CONDITION condition;
        public int conditionValue;
        public bool isActivated;

        public bool isCheck;
    }

    private record Effect
    {
        public RELIC_EFFECT effect;
        public int effectValue;
        public TARGET_TYPE targetType;
        public int targetCount;
    }
    
    private RelicTableData relicTableData;
    
    private List<Condition> _conditions = new();
    private List<Effect> _effects = new();
    private LOGICAL_OPERATOR conditionLogic => relicTableData.condition_logic ?? LOGICAL_OPERATOR.LOGICAL_OPERATOR_AND;
    
    private bool IsAllConditionActivated
    {
        get
        {
            if (conditionLogic == LOGICAL_OPERATOR.LOGICAL_OPERATOR_AND)
            {
                return _conditions.All(_ => _.isActivated);
            }
            else
            {
                return true;
            }
        }
    }

    private int conditionCount
    {
        get {
            int count = 0;
            if(relicTableData.active_condition_1 != null)
                ++count;
            if (relicTableData.active_condition_2 != null)
                ++count;
            return count;
        }
    }
    
    private int effectCount
    {
        get {
            int count = 0;
            if(relicTableData.relic_effect_1 != null)
                ++count;
            if (relicTableData.relic_effect_2 != null)
                ++count;
            if (relicTableData.relic_effect_3 != null)
                ++count;
            return count;
        }
    }
    
    public Relic(RelicTableData relicTableData)
    {
        this.relicTableData = relicTableData;
        for(int i  = 0; i< conditionCount; ++i)
        {
            switch (i)
            {
                case 0:
                {
                    var condition = new Condition
                    {
                        condition = relicTableData.active_condition_1 ?? ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START,
                        conditionValue = relicTableData.active_value_1 ?? 0,
                        isActivated = false
                    };
                    _conditions.Add(condition);
                    break;
                }
                case 1:
                {
                    var condition = new Condition
                    {
                        condition = relicTableData.active_condition_2 ?? ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START,
                        conditionValue = relicTableData.active_value_2 ?? 0,
                        isActivated = false
                    };
                    _conditions.Add(condition);
                    break;
                }
            }
        }

        for (int i = 0; i < effectCount; ++i)
        {
            Effect effect = new Effect();
            switch (i)
            {
                case 0:
                    effect.effect = relicTableData.relic_effect_1 ?? RELIC_EFFECT.RELIC_EFFECT_ARMOR;
                    effect.effectValue = relicTableData.effect_value_1 ?? 0;
                    effect.targetType = relicTableData.target_1 ?? TARGET_TYPE.TARGET_TYPE_SELF;
                    effect.targetCount = relicTableData.target_count_1 ?? 0;
                    break;
                case 1:
                    effect.effect = relicTableData.relic_effect_2 ?? RELIC_EFFECT.RELIC_EFFECT_ARMOR;
                    effect.effectValue = relicTableData.effect_value_2 ?? 0;
                    effect.targetType = relicTableData.target_2 ?? TARGET_TYPE.TARGET_TYPE_SELF;
                    effect.targetCount = relicTableData.target_count_2 ?? 0;
                    break;
                case 2:
                    effect.effect = relicTableData.relic_effect_3 ?? RELIC_EFFECT.RELIC_EFFECT_ARMOR;
                    effect.effectValue = relicTableData.effect_value_3 ?? 0;
                    effect.targetType = relicTableData.target_3 ?? TARGET_TYPE.TARGET_TYPE_SELF;
                    effect.targetCount = relicTableData.target_count_3 ?? 0;
                    break;
            }
            _effects.Add(effect);
        }
    }

    public void RegisterEvent()
    {
        var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        
        if (relicTableData.active_condition_1 == ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START
            || relicTableData.active_condition_2 == ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START)
        {
            // 게임 시작시
            gameMode.BattleHandler.OnGameStart += OnBattleStart;
            gameMode.BattleHandler.OnGameStart += OnHPPercentMinus;
            gameMode.BattleHandler.OnGameStart += OnHPMinus;
        }
        else if (relicTableData.active_condition_1 == ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_END ||
                 relicTableData.active_condition_2 == ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_END)
        {
            gameMode.MapHandler.OnGameEnd += OnBattleEnd;
            gameMode.MapHandler.OnGameEnd += OnHPPercentMinus;
            gameMode.MapHandler.OnGameEnd += OnHPMinus;
            // 게임 종료시
        }
        else if (relicTableData.active_condition_1 == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT ||
                 relicTableData.active_condition_2 == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT)
        {
            // 데미지시
            gameMode.PlayerActorHandler.player.data.OnEventDamaged += OnHPPercentMinus;
        }
        else if (relicTableData.active_condition_1 == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE ||
                 relicTableData.active_condition_2 == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE)
        {
            gameMode.PlayerActorHandler.player.data.OnEventDamaged += OnHPMinus;
            // 데미지시
        }
        
    }
    
    public void UnregisterEvent()
    {
        var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();

        if (relicTableData.active_condition_1 == ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START
            || relicTableData.active_condition_2 == ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_START)
        {
            // 게임 시작시
            // 게임 시작시
            gameMode.BattleHandler.OnGameStart -= OnBattleStart;
            gameMode.BattleHandler.OnGameStart -= OnHPPercentMinus;
            gameMode.BattleHandler.OnGameStart -= OnHPMinus;
        }
        else if (relicTableData.active_condition_1 == ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_END ||
                 relicTableData.active_condition_2 == ACTIVE_CONDITION.ACTIVE_CONDITION_BATTLE_END)
        {
            gameMode.MapHandler.OnGameEnd -= OnBattleEnd;
            gameMode.MapHandler.OnGameEnd -= OnHPPercentMinus;
            gameMode.MapHandler.OnGameEnd -= OnHPMinus;
        }
        else if (relicTableData.active_condition_1 == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT ||
                 relicTableData.active_condition_2 == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT)
        {
            // 데미지시
            gameMode.PlayerActorHandler.player.data.OnEventDamaged -= OnHPPercentMinus;
        }
        else if (relicTableData.active_condition_1 == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE ||
                 relicTableData.active_condition_2 == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE)
        {
            gameMode.PlayerActorHandler.player.data.OnEventDamaged -= OnHPMinus;
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
        if (gameMode.PlayerActorHandler.player.data.GetHp() <= condition.conditionValue)
        {
            isActivated = true;
        }
        
        ActivateCondition(ACTIVE_CONDITION.ACTIVE_CONDITION_HP_MINUS_VALUE, isActivated);
    }

    private void OnHPPercentMinus()
    {
        bool isActivated = false;
        var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var hpPercnet = (float)gameMode.PlayerActorHandler.player.data.GetHp()/  gameMode.PlayerActorHandler.player.data.GetMaxHp();
        hpPercnet *= 100;
        var condition = _conditions.Find(_ => _.condition == ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT);
        if (hpPercnet <= condition?.conditionValue)
        {
            isActivated = true;
        }
        ActivateCondition(ACTIVE_CONDITION.ACTIVE_CONDITION_HP_PERCENT, isActivated);
    }

    private void ActivateCondition(ACTIVE_CONDITION conditionType, bool isActivated)
    {
        var condition = _conditions.Find(_ => _.condition == conditionType);
        if (condition == null)
            return;
        
        condition.isActivated = isActivated;
        condition.isCheck = true;
        
        bool isAllCheck = _conditions.All(_=>_.isCheck == false);

        if (IsAllConditionActivated)
        {
            for (int i = 0; i < effectCount; ++i)
            {
                DoEffect(_effects[i]);
            }
        }

        if (isAllCheck)
        {
            _conditions.ForEach(_ => _.isActivated = false);
            _conditions.ForEach(_=>_.isCheck = false);
        } 
    }

    private void DoEffect(Effect effect)
    {
        Debug.Log("DoEffect"+ effect.effect + " 타겟 "+effect.targetType );
        switch (effect.effect)
        {
            case RELIC_EFFECT.RELIC_EFFECT_HEAL:
                DoEffectHeal(effect.targetType, effect.targetCount, effect.effectValue);
                break;
            case RELIC_EFFECT.RELIC_EFFECT_ARMOR:
                DoEffectAmor(effect.targetType, effect.targetCount, effect.effectValue);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
        }   
    }
    
    public void DoEffectHeal(TARGET_TYPE targetType, int targetCount, int value )
    {
        var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();

        if (targetType == TARGET_TYPE.TARGET_TYPE_SELF)
        {
            gameMode.PlayerActorHandler.player.data.DoHeal(value);
        }
        else if (targetType == TARGET_TYPE.TARGET_TYPE_ENEMY)
        {
            gameMode.ActorSpawner.GetEnemy(0).data.DoHeal(value);
            
        }
    }
    
    public void DoEffectAmor(TARGET_TYPE targetType, int targetCount,int value )
    {
        var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();

        if (targetType == TARGET_TYPE.TARGET_TYPE_SELF)
        {
            gameMode.PlayerActorHandler.player.data.AddAmorStat(value);
        }
        else if (targetType == TARGET_TYPE.TARGET_TYPE_ENEMY)
        {
            gameMode.ActorSpawner.GetEnemy(0).data.AddAmorStat(value);
        }
    }

    public string GetImage()
    {
        return relicTableData.relic_resource;
    }
    
    public string GetDesc()
    {
        return GameUtil.GetString(relicTableData.relic_desc);
    }

    public string GetName()
    {
        return GameUtil.GetString(relicTableData.relic_name);
    }
}
