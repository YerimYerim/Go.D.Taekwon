using UnityEngine;

public static class GameUtil
{
    public static int PLAYER_ACTOR_ID = 1000001;
    public static string ENEMY_PARENT_NAME = "Enemy";
    public static string PLAEYER_PARENT_NAME = "Actor";
    
    public static string GetString(string key)
    {
        var stringdata = GameTableManager.Instance._stringDatas.Find(_ => _.string_key.Equals(key));
        if(stringdata != null)
        {
            switch (GameSettingManager.Instance.languageType)
            {
                case LANGUAGE_TYPE.LANGUAGE_TYPE_KOR:
                    return stringdata.value_kor.Replace("\\n", "\n");
                    break;
                default:
                    return "??";
            }
        }
        else
        {
            return key+"??";
        }
        return string.Empty;
    }


    public static int NextRingIndex(int cur,  int max, int min= 0)
    {
        var nextIndex = cur + 1;
        return nextIndex >= max ? min : nextIndex;
    }
    
    public static void Log(string log)
    {
        Debug.Log(log);
    }
}
