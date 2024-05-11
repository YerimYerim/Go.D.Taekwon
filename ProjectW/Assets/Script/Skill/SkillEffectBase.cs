using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffectBase
{
    protected int effectId;
    protected SpellEffectTableData table;
    
    public abstract void DoSkill(List<GameActor> targetActor,  GameActor myActor);
    public abstract void InitSkillType(SpellEffectTableData data);
    public abstract int GetValue();
}
