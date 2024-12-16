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
        var attackStat = myActor.data.GetAttackStat();
        for(int i = 0; i< targetActor.Count; ++i)
        {
            targetActor[i].data.TakeDamage(GetValue()  * Mathf.CeilToInt(1 + attackStat.DamageUpPer * 0.01f + attackStat.DamageUpFixed), attackStat.TrueDamageUp);
            targetActor[i].OnUpdateHp(targetActor[i].data);
        }
        myActor?.data.GiveDamage(GetValue()  * Mathf.CeilToInt(1 +  attackStat.DamageUpPer * 0.01f + attackStat.DamageUpFixed));
    }

    public void DoDamage(GameActor enemy)
    {
        
    }
}
