using System;
using System.Collections.Generic;
using System.Linq;

public class ActorEnemyData : ActorDataBase
{
    // phase , data
    private Dictionary<int, SkillPatternGroupTableData> patternGroup = new();
    
    // phase, data
    private Dictionary<int,List<SkillGroupTableData>> skillGroup = new();
    
    // effect_id, skillbase 
    private MonsterSkillBase _skillBase;

    private int curPhase = 1;

    private bool IsDoingSkill = false;

    private SkillGroupTableData _latestSkill;
    

    public void InitAP(int skillPatternGroupId)
    {
        curPhase = 1;
        _latestSkill = null;
        // Init 삼형제 순서 중요.
        InitPatternGroup(skillPatternGroupId);
        InitSkillGroup();
        StartSkillCoolTime();
    }

    private void StartSkillCoolTime()
    {
        var skillGroupTable = GetSkill();
        _skillBase = new MonsterSkillBase(skillGroupTable?.action_point ?? 0);
        
    }
    private void InitSkillGroup()
    {
        var patternGroupValue = patternGroup.Values.ToList();
        for (int i = 0; i < patternGroupValue.Count; ++i)
        {
            var skillGroupTable = GameDataManager.Instance._skillGroupTableDatas.FindAll(_ => _.skill_group_id == patternGroupValue[i].skill_group);
            skillGroup.Add(patternGroupValue[i].phase ?? 0, skillGroupTable);
        }
    }
    private void InitPatternGroup(int skillPatternGroupId)
    {
        var patternGroupTable = GameDataManager.Instance._patternGroupTableDatas.FindAll(_ => _.skill_pattern_group_id == skillPatternGroupId);
        patternGroupTable.Sort((a, b) => (a?.phase ?? 0).CompareTo(b?.phase ?? 0));

        for (int i = 0; i < patternGroupTable.Count; ++i)
        {
            if (patternGroup.ContainsKey(patternGroupTable[i]?.phase ?? 0))
            {
                patternGroup[patternGroupTable[i]?.phase ?? 0] = patternGroupTable[i];
            }
            else
            {
                patternGroup.Add(patternGroupTable[i]?.phase ?? 0, patternGroupTable[i]);
            }
        }
    }
    
    
    public bool IsCanUseSkill()
    {
        return _skillBase.CurrentAP <= 0;
    }

    public void CalculatePhase()
    {
        bool isAddPhase = IsPhaseAddCondition(patternGroup[curPhase].phase_condition, patternGroup[curPhase].phase_condition_value);
        if (isAddPhase)
        {
            ++curPhase;
        }
    }
    

    public SkillGroupTableData GetSkill()
    {
        //if(IsCanUseSkill())
        {
            switch (patternGroup[curPhase].pattern_type)
            {
                case PATTERN_TYPE.PATTERN_TYPE_RANDOM:
                {
                    uint maxIndex = (uint) skillGroup[curPhase].Count;

                    var index = Unity.Mathematics.Random.CreateFromIndex(maxIndex);
                    SkillGroupTableData skillGroupTableData = skillGroup[curPhase][index.NextInt((int) maxIndex)];
                    return skillGroupTableData;
                }
                    break;
                case PATTERN_TYPE.PATTERN_TYPE_SEQUENTIAL:
                {
                    var skillGroupTableDataIndex = skillGroup[curPhase].FindIndex(_ => _ == _latestSkill);
                    var nextIndex = GameUtil.NextRingIndex(skillGroupTableDataIndex, skillGroup[curPhase].Count);
                    return skillGroup[curPhase][nextIndex];
                }
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return null;
    }  

    private bool IsPhaseAddCondition(PHASE_CONDITION? phaseCondition, float? value)
    {
        switch (phaseCondition)
        {
            case PHASE_CONDITION.PHASE_CONDITION_NONE:
                return false;
                break;
            case PHASE_CONDITION.PHASE_CONDITION_HP_CONDITION:
                if (Hp < MaxHp * value)
                {
                    return true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(phaseCondition), phaseCondition, null);
        }

        return false;
    }
    public void MinusAP(int minusAp)
    {
        _skillBase.ReduceAP(minusAp);
    }
    public void ResetAP()
    {
        _skillBase.ResetAp();
    }
    public void AddAP(int addAp)
    {
        _skillBase.GainAP(addAp);
    }
    
    public int GetRemainAp()
    {
        return _skillBase.CurrentAP;
    }
}
