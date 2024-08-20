using System;
using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public static class GameUtil
{
    public static int PLAYER_ACTOR_ID = 1000001;
    public static string ENEMY_PARENT_NAME = "Enemy";
    public static string PLAEYER_PARENT_NAME = "Actor";

    
    
    
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
            case EFFECT_TYPE.EFFECT_TYPE_HEAL:
            {
                SkillHealType skillHealType = new SkillHealType();
                skillHealType.InitSkillType(data);
                return skillHealType;
            } break;
            case EFFECT_TYPE.EFFECT_TYPE_DOT_SKILL_DAMAGE:
            {
                SkillDotDamageType skillDotDamageType = new SkillDotDamageType();
                skillDotDamageType.InitSkillType(data);
                return skillDotDamageType;
            } break;
            case EFFECT_TYPE.EFFECT_TYPE_DOT_HEAL:
            {
                SkillDotHealType skillDotHealType = new SkillDotHealType();
                skillDotHealType.InitSkillType(data);
                return skillDotHealType;
            } break;
            case EFFECT_TYPE.EFFECT_TYPE_DRAW:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_ARMOR:
            {
                SkillAmorUpType skillAmorUpType= new SkillAmorUpType();
                skillAmorUpType.InitSkillType(data);
                return skillAmorUpType;
            } break;
            case EFFECT_TYPE.EFFECT_TYPE_ARMOR_DOWN:
            {
                SkillAmorDownType skillAmorDownType= new SkillAmorDownType();
                skillAmorDownType.InitSkillType(data);
                return skillAmorDownType;
            } break;
            case EFFECT_TYPE.EFFECT_TYPE_DEBUFF_DISPEL:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_BUFF_DISPEL:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_AP_UP:
            {
                SkillApUp skillApUp = new SkillApUp();
                skillApUp.InitSkillType(data);
                return skillApUp;
            } break;
            case EFFECT_TYPE.EFFECT_TYPE_AP_STOP:
            {
                
            }
                break;
            case EFFECT_TYPE.EFFECT_TYPE_ACTION_CANCEL:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_FUSION_LEVEL_UP:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_STUN:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_REFLECT_DAMAGE:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_REFLECT_DAMAGE_PER:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_DAMAGE_INVINCIBLE:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_AVOID_UP:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_AVOID_DOWN:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_ALL_UP_PER:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_ALL_DOWN_PER:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_DOT_DRAW:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_DEBUFF_ACCURACY_UP:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_DEBUFF_ACCURACY_DOWN:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_DEBUFF_RESISTANCE_UP:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_DEBUFF_RESISTANCE_DOWN:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_DAMAGE_SPREAD:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_PROVOKE:
                break;
            case EFFECT_TYPE.EFFECT_TYPE_COUNTER:
                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }
    

    public static int NextRingIndex(int cur,  int max, int min= 0)
    {
        var nextIndex = cur + 1;

        return nextIndex >= max ? min : nextIndex;
    }
}
