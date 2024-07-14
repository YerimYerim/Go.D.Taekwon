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

    public override int GetValue()
    {
        return damage;
    }

    public override void DoSkill(List<GameActor> targetActor,  GameActor myActor)
    {
        for(int i=0; i< targetActor.Count;++i)
        {
            targetActor[i].data.DoDamaged(damage);
            targetActor[i].OnUpdateHp();
        }
    }

    void ISkillTargetDamage.DoDamage(GameActor target)
    {
    }
}
