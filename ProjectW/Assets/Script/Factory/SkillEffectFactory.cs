public static class SkillEffectFactory
{
    public static SkillEffectBase GetSkillEffectBase(SpellEffectTableData data)
    {
        switch (data.effect_type)
        {
            case EFFECT_TYPE.EFFECT_TYPE_DAMAGE:
                return CreateSkillEffect<SkillDamageType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_HEAL:
                return CreateSkillEffect<SkillHealType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_DOT_SKILL_DAMAGE:
                return CreateSkillEffect<SkillDotDamageType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_DOT_HEAL:
                return CreateSkillEffect<SkillDotHealType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_ARMOR:
                return CreateSkillEffect<SkillAmorUpType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_ARMOR_DOWN:
                return CreateSkillEffect<SkillAmorDownType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_AP_UP:
                return CreateSkillEffect<SkillApUp>(data);
            case EFFECT_TYPE.EFFECT_TYPE_AP_STOP:
                return CreateSkillEffect<SkillApDownType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_DOT_AMOR:
                return CreateSkillEffect<SkillDotAmorType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_TAKE_DAMAGE_UP_BUFF:
                return CreateSkillEffect<SkillTakeDamageUpType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_TAKE_DAMAGE_DOWN_BUFF:
                return CreateSkillEffect<SkillTakeDamageDownType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_GIVE_DAMAGE_UP_BUFF:
                return CreateSkillEffect<SkillDamageUpType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_GIVE_DAMAGE_DOWN_BUFF:
                return CreateSkillEffect<SkillDamageDownType>(data);
            case EFFECT_TYPE.EFFECT_TYPE_IGNORE_DAMAGE:
                return CreateSkillEffect<SkillIgnoreDamage>(data);
            case EFFECT_TYPE.EFFECT_TYPE_INVALID_DAMAGE:
                return CreateSkillEffect<SkillStackIgnoreDamage>(data);
            case EFFECT_TYPE.EFFECT_TYPE_HEAL_PROPORTION_GIVEN_DAMAGE:
                return CreateSkillEffect<SkillHealProportionGivenDamage>(data);
            default:
                return null;
        }
    }

    private static T CreateSkillEffect<T>(SpellEffectTableData data) where T : SkillEffectBase, new()
    {
        T skillEffect = new T();
        skillEffect.InitSkillType(data);
        return skillEffect;
    }
}