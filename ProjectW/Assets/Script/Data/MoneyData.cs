using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyData : DataBase
{
    // money id / count
    private Dictionary<int, int> moneyData = new();
    
    public void AddMoney(int moneyId, int count)
    {
        if (moneyData.ContainsKey(moneyId))
        {
            moneyData[moneyId] += count;
        }
        else
        {
            moneyData.Add(moneyId, count);
        }
    }
    
    public int GetMoney(int moneyId)
    {
        return moneyData.TryGetValue(moneyId, value: out var value) ? value : 0;
    }
}
