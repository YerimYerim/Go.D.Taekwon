using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorEnemyData : ActorDataBase
{
    //public int ActionPoint { get; private set; }
    
    // phase , data
    private Dictionary<int, SkillPatternGroupTableData> patternGroup = new();
    
    // phase, data
    private Dictionary<int,List<SkillGroupTableData>> skillGroup = new();
    // effect_id, skillbase 
    private Dictionary<int , MonsterSkillBase> _skillBases = new();

    private int curPhase = 1;


    public void InitAP(int skillPatternGroupId)
    {
        curPhase = 1;
        
        // Init 삼형제 순서 중요.
        InitPatternGroup(skillPatternGroupId);
        InitSkillGroup();
        InitSkillBases();
    }

    private void InitSkillBases()
    {
        var skillGroupTable = skillGroup[curPhase];
        foreach (var skillGroupTableData in skillGroupTable)
        {
            try
            {
                _skillBases.Add( skillGroupTableData?.effect_id ?? 0 , new MonsterSkillBase(skillGroupTableData?.action_point ?? 0));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
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
            patternGroup.Add(patternGroupTable[i]?.phase ?? 0, patternGroupTable[i]);
        }
    }
    
    public bool IsCanUseSkill()
    {
        var enumerator = _skillBases.Values.ToList();
        for (int i = 0; i < enumerator.Count; ++i)
        {
            if (enumerator[i].CurrentAP < 0)
            {
                return true;
            }
        }

        return false;
    }
    public void MinusAP(int minusAp)
    {
        var enumerator = _skillBases.Values.ToList();
        for (int i = 0; i < enumerator.Count; ++i)
        {
            enumerator[i].ReduceAP(minusAp);
        }

    }
    public void ResetAP(int ap)
    {
        var enumerator = _skillBases.Values.ToList();
        for (int i = 0; i < enumerator.Count; ++i)
        {
            if (enumerator[i].CurrentAP < 0)
                enumerator[i].ResetAp();
        }
    }
    public void AddAP(int addAp)
    {
        var enumerator = _skillBases.Values.ToList();
        for (int i = 0; i < enumerator.Count; ++i)
        {
            enumerator[i].GainAP(addAp);
        }
    }
}
