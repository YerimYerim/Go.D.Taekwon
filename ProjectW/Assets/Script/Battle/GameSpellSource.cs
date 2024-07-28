using System;

public class GameSpellSource
{
    private int _maxAp;
    private int _remainAP;
    private int _sourceId;

    private SpellSourceTableData _tableData;

    // spell id
    public event Action<int> OnMakeSpellEvent;
    
    /// <summary>
    /// remainAP
    /// </summary>
    public event Action<int> OnUpdateUI;
    public void Init(int sourceID, Action<int> onMakeSpellEvent)
    {
        OnMakeSpellEvent = onMakeSpellEvent;
        _sourceId = sourceID;
        _tableData = GameDataManager.Instance._spellSourceTableDatas.Find(_ => _.source_id == sourceID);
        _maxAp = _tableData.product_ap ?? 0;
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
            OnMakeSpellEvent?.Invoke(_tableData?.spell_id ?? 0);
            OnUpdateUI?.Invoke(_remainAP);
        }

    }

}
