using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffectBase
{
    protected int effectId;
    protected SpellEffectTableData table;
    protected int remainTurn;
    
    public abstract void DoSkill(List<GameActor> targetActor,  GameActor myActor);
    public abstract void InitSkillType(SpellEffectTableData data);
    public abstract int GetValue();

    public bool IsNotRemainTurn()
    {
        return remainTurn <= 0;
    }
    public SpellEffectTableData GetTable()
    {
        return table;
    }
}
