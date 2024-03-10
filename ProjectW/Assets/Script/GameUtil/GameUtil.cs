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
}
