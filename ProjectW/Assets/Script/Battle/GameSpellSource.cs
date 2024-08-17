using System;

public class GameSpellSource
{
    private int _maxAp;
    private int _remainAP;
    private int _sourceId;
    private int _productionAmount;
    private int _productionSpellId;
    
    private SpellSourceTableData _tableData;

    // spell id
    public event Action<int, int > OnMakeSpellEvent;
    
    /// <summary>
    /// remainAP
    /// </summary>
    public event Action<int> OnUpdateUI;
    
    // id, amount 
    public void Init(int sourceID, Action<int, int> onMakeSpellEvent)
    {
        OnMakeSpellEvent = onMakeSpellEvent;
        _sourceId = sourceID;
        _tableData = GameDataManager.Instance._spellSourceTableDatas.Find(_ => _.source_id == sourceID);
        _maxAp = _tableData.product_ap ?? 0;
        _productionAmount = _tableData.product_value?? 0;
        _productionSpellId = _tableData.spell_id ?? 0;
        _remainAP = _maxAp;
    }

    public void ResetAp()
    {
        _remainAP = _maxAp;
    }

    public void UpdateAP()
    {
        if (_remainAP > 0)
        {
            --_remainAP;
            OnUpdateUI?.Invoke(_remainAP);
        }
        else
        {
            ResetAp();
            OnMakeSpellEvent?.Invoke(GetProductionSpellId(), _productionAmount);
            OnUpdateUI?.Invoke(_remainAP);
        }

    }
    
    public int GetSourceId()
    {
        return _sourceId;
    }

    public void SetMaxAp(int ap)
    {
        _maxAp = ap;
    }
    
    public void SetRemainAp(int ap)
    {
        _remainAP = ap;
    }
    
    public int GetMaxAp()
    {
        return _maxAp;
    }
    
    public void SetTableMaxAp()
    {
        SetMaxAp(_tableData.product_ap ?? 0);
    }
    
    public void SetTableProductionAmount()
    {
        _productionAmount = _tableData.product_value ?? 0;
    }
    
    public int GetProductionAmount()
    {
        return _productionAmount;
    }
    
    public void SetProductionAmount(int amount)
    {
        _productionAmount = amount;
    }
    
    public int GetProductionSpellId()
    {
        return _productionSpellId;
    }
    
    public void SetProductionSpellId(int spellId)
    {
        _productionSpellId = spellId;
    }
    
    public void SetTableProductionSpellId()
    {
        _productionSpellId = _tableData.spell_id ?? 0;
    }
    
    public int GetRemainAp()
    {
        return _remainAP;
    }

    /// <summary>
    /// _remainAP 감소
    /// </summary>
    public void ReduceAP(int amount)
    {
        for(int i = 0; i< amount; ++i)
            UpdateAP();
    }
}
