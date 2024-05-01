using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtil
{
    public static string GetString(string key)
    {
        var stringdata = GameDataManager.Instance._stringDatas.Find(_ => _.string_key.Equals(key));
        switch (GameSettingManager.Instance.languageType)
        {
            case LANGUAGE_TYPE.LANGUAGE_TYPE_KOR:
                return stringdata.value_kor;
                break;
            default:
                return "??";
        }
        
        return string.Empty;
    }

    public static SkillEffectBase GetSkillEffectBase(SpellEffectTableData data)
    {
        switch (data.effect_type)
        {
            case EFFECT_TYPE.EFFECT_TYPE_DAMAGE:
            {
                SkillDamageType skillDamageType = new SkillDamageType();
                skillDamageType.InitSkillType(data);
                return skillDamageType;
            } break;
        }

        return null;
    }
}
