using System;
using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class MoneyData : Singleton<MoneyData>
{
    // money id / count
    private Dictionary<int, int> moneyData = new();
    /// <summary>
    /// MoneyId/ amount 
    /// </summary>
    public event Action<int, int>  OnMoneyChanged;
    
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
        
        OnMoneyChanged?.Invoke(moneyId, moneyData[moneyId]);
    }
    
    public int GetMoney(int moneyId)
    {
        return moneyData.TryGetValue(moneyId, value: out var value) ? value : 0;
    }
}
