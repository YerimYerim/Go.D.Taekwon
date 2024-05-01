using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkillDamageType : SkillEffectBase, ISkillTargetDamage
{
    private int damage;
    
    public override void InitSkillType(SpellEffectTableData data)
    {
        effectId = data.effect_id ?? 0;
        table = data;
        damage = data.value_1 ?? 0;
    }

    public override int GetDamage()
    {
        return damage;
    }

    public override void DoSkill()
    {
    }

    void ISkillTargetDamage.DoDamage(GameActor target)
    {
    }
}
