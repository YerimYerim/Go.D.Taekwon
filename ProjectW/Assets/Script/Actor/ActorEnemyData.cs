using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

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

    public ActorEnemyData(MonsterTableData data)
    {
       Init(data?.stat_hp ?? 0);
       InitAP(data?.skill_pattern_group ?? 0);
       
       // Init 삼형제 순서 중요.
       InitPatternGroup(data?.skill_pattern_group ?? 0);
       InitSkillGroup();
       StartSkillCoolTime();
    }

    public void InitAP(int skillPatternGroupId)
    {
        curPhase = 1;
        _latestSkill = null;
    }

    public void StartSkillCoolTime()
    {
        var skillGroupTable = StartSkill();
        _skillBase = new MonsterSkillBase(skillGroupTable?.action_point ?? 0)
        {
            skill_group_id = skillGroupTable?.skill_group_id ?? 0,
            effect_id = skillGroupTable?.effect_id,
        };
    }
    private void InitSkillGroup()
    {
        var patternGroupValue = patternGroup.Values.ToList();
        foreach (var patter in patternGroupValue)
        {
            var skillGroupTable = GameTableManager.Instance._skillGroupTableDatas.FindAll(_ => _.skill_group_id == patter.skill_group);
            skillGroup.Add(patter.phase ?? 0, skillGroupTable);
        }
    }
    private void InitPatternGroup(int skillPatternGroupId)
    {
        var patternGroupTable = GameTableManager.Instance._patternGroupTableDatas.FindAll(_ => _.skill_pattern_group_id == skillPatternGroupId);
        patternGroupTable.Sort((a, b) => (a?.phase ?? 0).CompareTo(b?.phase ?? 0));

        foreach (var pattern in patternGroupTable)
        {
            int phase = pattern?.phase ?? 0;
            if (patternGroup.TryAdd(phase, pattern) == false)
            {
                patternGroup[phase] = pattern;
            }
        }
    }
    
    
    public bool IsCanUseSkill()
    {
        return _skillBase.CurrentAP <= 0;
    }

    public int[] GetSkillID()
    {
        return _skillBase?.effect_id;
    }

    private SkillGroupTableData StartSkill()
    {
        switch (patternGroup[curPhase].pattern_type)
        {
            case PATTERN_TYPE.PATTERN_TYPE_RANDOM:
            {
                int maxIndex = skillGroup[curPhase].Count;
                var index = Random.Range(0,maxIndex);
                SkillGroupTableData skillGroupTableData = skillGroup[curPhase][index];
                _latestSkill = skillGroup[curPhase][index];
                return skillGroupTableData;
            } break;
            case PATTERN_TYPE.PATTERN_TYPE_SEQUENTIAL:
            {
                var skillGroupTableDataIndex = skillGroup[curPhase].FindIndex(_ => _ == _latestSkill);
                var nextIndex = GameUtil.NextRingIndex(skillGroupTableDataIndex, skillGroup[curPhase].Count);
                _latestSkill = skillGroup[curPhase][nextIndex];
                return skillGroup[curPhase][nextIndex];
            } break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return null;
    }  
    public void MinusAP(int minusAp)
    {
        _skillBase.ReduceAP(minusAp);
    }
    public void ResetAP()
    {
        StartSkillCoolTime();
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
    
    public int GetMaxAp()
    {
        return _skillBase.MaxAP;
    }
}
