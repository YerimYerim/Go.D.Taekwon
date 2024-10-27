using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    private Dictionary<REWARD_TYPE, DataBase> data = new();
    public new void Init()
    {
        base.Init();
        
        data.Add(REWARD_TYPE.REWARD_TYPE_MONEY, new MoneyData());
    }
    
    public DataBase GetData(REWARD_TYPE type)
    {
        return data.TryGetValue(type, out var value) ? value : null;
    }
}
