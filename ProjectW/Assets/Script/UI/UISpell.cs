using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using Script.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpell : UIDragable
{
    [SerializeField] private Image img;
    [SerializeField] private DTButton button;
    public UICardDeckOnHand _parents;
    public UICardDeckOnHand.SpellData _spellTableData { get; private set; }
    public void SetUI(UICardDeckOnHand.SpellData spellTableData)
    {
        _spellTableData = spellTableData;
        img.sprite = GameResourceManager.Instance.GetImage(spellTableData.tableData.spell_img);
        button.SetHoverEvent(ShowToolTip, HideToolTip);
        InitDragSuccessCondition(ActionFail, ActionSuccess, AdjustIsMerge, OnEventDrag);
    }

    private bool AdjustIsMerge(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var isSpellUI = result.gameObject.GetComponent<UISpell>();
            if (isSpellUI!= this && isSpellUI != null && isSpellUI.isActiveAndEnabled == true)
            {
                var spellCombineData = GameDataManager.Instance._spellCombineDatas.FindAll(_ => _.material_1 == _spellTableData.tableData.spell_id || _.material_2 == _spellTableData.tableData.spell_id);
                var resultCombineSpell = spellCombineData.Find(_ => _.material_1 == isSpellUI._spellTableData.tableData.spell_id || _.material_2 == isSpellUI._spellTableData.tableData.spell_id);
                if (resultCombineSpell == null)
                    return false;
                SpellTableData resultSpell = GameDataManager.Instance._spellData.Find(_ => _.spell_id == resultCombineSpell.result_spell);
                if (resultSpell == null)
                    return false;
                return true;
            }
        }
        return false;
    }

    private void ActionSuccess(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var isSpellUI = result.gameObject.GetComponent<UISpell>();
            if (isSpellUI!= this && isSpellUI != null && isSpellUI.isActiveAndEnabled == true)
            {
                var spellCombineData = GameDataManager.Instance._spellCombineDatas.FindAll(_ => _.material_1 == _spellTableData.tableData.spell_id || _.material_2 == _spellTableData.tableData.spell_id);
                var resultCombineSpell = spellCombineData.Find(_ => _.material_1 == isSpellUI._spellTableData.tableData.spell_id || _.material_2 == isSpellUI._spellTableData.tableData.spell_id);
                if (resultCombineSpell == null)
                    return ;
                SpellTableData resultSpell = GameDataManager.Instance._spellData.Find(_ => _.spell_id == resultCombineSpell.result_spell);
                if (resultSpell == null)
                    return;
                MoveReset();
                _parents.MergeSpell(isSpellUI._spellTableData?.tableData?.spell_id ?? 0, _spellTableData?.tableData?.spell_id ?? 0, resultSpell.spell_id ?? 0 );
            }
        }
    }

    private void ActionFail(PointerEventData eventData)
    {
        MoveReset();
    }

    private void OnEventDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        HideToolTip();
        foreach (var result in results)
        {
            var isSpellUI = result.gameObject.GetComponent<UISpell>();
            if (isSpellUI!= this && isSpellUI != null && isSpellUI.isActiveAndEnabled == true)
            {
                var spellCombineData = GameDataManager.Instance._spellCombineDatas.FindAll(_ => _.material_1 == _spellTableData.tableData.spell_id || _.material_2 == _spellTableData.tableData.spell_id);
                var resultCombineSpell = spellCombineData.Find(_ => _.material_1 == isSpellUI._spellTableData.tableData.spell_id || _.material_2 == isSpellUI._spellTableData.tableData.spell_id);
                if (resultCombineSpell == null)
                    return;
                
                var resultSpell = GameDataManager.Instance._spellData.Find(_ => _.spell_id == resultCombineSpell.result_spell);
                
                if (resultSpell == null)
                    return;
                if(GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_2, out var ui))
                {
                    ui.CreateInfo(resultSpell.spell_name, resultSpell.spell_desc, this.transform.GetComponent<RectTransform>());
                    ui.Show();
                }
            }
        }
    }
    private void ShowToolTip()
    {
        if(GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_2, out var ui))
        {
            ui.CreateInfo(_spellTableData.tableData.spell_name, _spellTableData.tableData.spell_desc, this.transform.GetComponent<RectTransform>());
            ui.Show();
        }
    }

    private void HideToolTip()
    {
        if(GameUIManager.Instance.TryGet<UITooltip>(out var ui))
        {
            ui.Hide();
        }
    }
}
