using System;

public class GameSpellSource
{
    private int _maxAp;
    private int _remainAP;
    private int _sourceId;
    private int _productionAmount;
    private int _productionSpellId;
    private int _productionInitAmount;
    
    private SpellSourceTableData _tableData;

    // spell id
    public event Action<int, int > OnMakeSpellEvent;
    public event Action OnUpdateGauge;

    // id, amount 
    public void Init(SpellSourceTableData sourceTable, Action<int, int> onMakeSpellEvent, Action onUpdateGauge)
    {
        OnMakeSpellEvent = onMakeSpellEvent;
        OnUpdateGauge = onUpdateGauge;
        _sourceId = sourceTable?.source_id ?? 0;
        _tableData = sourceTable;
        _maxAp = sourceTable?.product_ap ?? 0;
        _productionAmount = sourceTable?.product_value?? 0;
        _productionSpellId = sourceTable?.spell_id ?? 0;
        _remainAP = _maxAp;
        _productionInitAmount = sourceTable?.product_value_init ?? 0;
    }

    public void ResetAp()
    {
        _remainAP = _maxAp;
    }

    public void ReduceAp(int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            if (_remainAP > 0)
            {
                --_remainAP;
                if (_remainAP <= 0)
                {
                    //OnUpdateGauge?.Invoke();
                    ResetAp();
                    OnMakeSpellEvent?.Invoke(GetProductionSpellId(), _productionAmount);
                }
            }
        }
    }

    private void AddAp()
    {
        ++_remainAP;
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
    public int GetInitProductionAmount()
    {
        return _productionInitAmount;
    }
    /// <summary>
    /// _remainAP 증가
    /// </summary>
    public void AddAP(int amount)
    {
        for(int i = 0; i< amount; ++i)
            AddAp();
    }
    public string GetSourceImage()
    {
        var img = GameTableManager.Instance._spellData.Find(_ => _.spell_id == _productionSpellId)?.spell_img;
        return img;
    }   
    
    public string GetSourceBgImage()
    {
        var img = GameTableManager.Instance._spellSourceTableDatas.Find(_ => _.source_id == _sourceId).support_module_bg;
        return img;
    }
    public string GetSourceIconImage()
    {
        var img = GameTableManager.Instance._spellSourceTableDatas.Find(_ => _.source_id == _sourceId).source_img;
        return img;
    }

    public string GetSourceName()
    {
        var name = GameTableManager.Instance._spellData.Find(_ => _.spell_id == _productionSpellId)?.spell_name;
        return name;
    }
    
    public string GetSourceDesc()
    {
        var desc = GameTableManager.Instance._spellData.Find(_ => _.spell_id == _productionSpellId)?.spell_desc;
        return desc;
    }
}
