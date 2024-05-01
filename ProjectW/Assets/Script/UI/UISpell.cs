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
    public GameActor selectedActor;
    public GameDeckManager.SpellData _spellTableData { get; private set; }
    public void SetUI(GameDeckManager.SpellData spellTableData)
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
        
        
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 2f);
        var colideObject = Physics2D.RaycastAll(ray.origin, ray.direction);
        if (selectedActor != null)
        {
            selectedActor.OnDeselected();
            selectedActor = null;
        }
            
        foreach (var hit in colideObject)
        {
            var actor = hit.transform.GetComponent<GameActor>();
            Debug.Log(hit.collider.name);
            if (actor != null)
            {
                selectedActor = actor;
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
        
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 2f);
        var colideObject = Physics2D.RaycastAll(ray.origin, ray.direction);

        foreach (var hit in colideObject)
        {
            var actor = hit.transform.GetComponent<GameActor>();
            if (actor != null && actor.isActiveAndEnabled)
            {
                // 아군 적군 구분 효과 필요하긴함 
                var spellEffect = _spellTableData.tableData.spell_effect;
                for (int i = 0; i < spellEffect.Length; ++i)
                {
                    var effect = GameDataManager.Instance._spelleffectDatas.Find(_ => spellEffect[i] == _.effect_id);
                    GameBattleManager.Instance.Attack(this._spellTableData.tableData.spell_id ??0, effect, GameBattleManager.Instance.enemy);

                }
            }
        }
        MoveReset();
    }

    private void ActionFail(PointerEventData eventData)
    {
        MoveReset();
        if(selectedActor!= null)
        {
            selectedActor.OnDeselected();
            selectedActor = null;
        }
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
        
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 2f);
        var colideObject = Physics2D.RaycastAll(ray.origin, ray.direction);
        if (selectedActor != null)
        {
            selectedActor.OnDeselected();
            selectedActor = null;
        }
            
        for (int i = 0; i < colideObject.Length; ++i)
        {
            var hit = colideObject[i];
            var actor = hit.transform.GetComponent<GameActor>();
            Debug.Log(hit.collider.name);
            if (actor != null)
            {
                selectedActor = actor;
                actor.OnSelected();
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
