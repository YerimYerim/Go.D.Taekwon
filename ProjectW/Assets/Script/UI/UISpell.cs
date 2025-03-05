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
    private int _index = 0;
    public void SetUI(GameDeckManager.SpellData spellTableData, int index)
    {
        _spellTableData = spellTableData;
        img.sprite = GameResourceManager.Instance.GetImage(spellTableData.tableData.spell_img);
        button.SetHoverEvent(ShowToolTip, HideToolTip);
        InitDragSuccessCondition(IsActionFail, IsActionSuccess, AdjustIsMerge, OnEventDrag);
    }
    public void SetIndex(int index)
    {
        _index = index;
    }
    private bool AdjustIsMerge(PointerEventData eventData)
    {
        var isSpellUI = FindUISpell(eventData);
        if (isSpellUI != null)
        {
            var spellCombineData = GameTableManager.Instance._spellCombineDatas.Find(_ => (_.material_1 == _spellTableData.tableData.spell_id && _.material_2 == isSpellUI._spellTableData.tableData.spell_id));
            spellCombineData ??= GameTableManager.Instance._spellCombineDatas.Find(_ =>_.material_1 == isSpellUI._spellTableData.tableData.spell_id && _.material_2  == _spellTableData.tableData.spell_id );
            if (spellCombineData == null)
                return false;
            SpellTableData resultSpell = GameTableManager.Instance._spellData.Find(_ => _.spell_id == spellCombineData.result_spell);
            if (resultSpell == null)
                return false;
            return true;
        }

        var actor = FindGameActor(eventData);
        if (actor != null)
        {
            selectedActor = actor;
            return true;
        }

        return false;
    }

    private void IsActionSuccess(PointerEventData eventData)
    {
        UISpell otherSpellUI = FindUISpell(eventData);
        if (otherSpellUI != null)
        {
            var spellCombineData = GameTableManager.Instance._spellCombineDatas.Find(_ => (_.material_1 == _spellTableData.tableData.spell_id && _.material_2 == otherSpellUI._spellTableData.tableData.spell_id));
            spellCombineData ??= GameTableManager.Instance._spellCombineDatas.Find(_ =>_.material_1 == otherSpellUI._spellTableData.tableData.spell_id && _.material_2  == _spellTableData.tableData.spell_id );

            if (spellCombineData == null)
                return;
            SpellTableData resultSpell = GameTableManager.Instance._spellData.Find(_ => _.spell_id == spellCombineData.result_spell);
            if (resultSpell == null)
                return;
            var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
            if (gameBattleMode == null)
            {
                return;
            }
            _parents.MergeSpell(_index, otherSpellUI._index,  resultSpell.spell_id ?? 0);
        }
        else
        {
            GameActor actor = FindGameActor(eventData);
            if (actor != null && actor.isActiveAndEnabled)
            {
                var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
                if (gameBattleMode == null)
                {
                    return;
                }
                gameBattleMode.BattleHandler.DoSkill(_spellTableData, actor);
            }
            if (selectedActor != null)
            {
                selectedActor.OnDeselected();
                selectedActor = null;
            }
        }
        
        MoveReset();
    }

    private void IsActionFail(PointerEventData eventData)
    {
        MoveReset();
        if (selectedActor != null)
        {
            selectedActor.OnDeselected();
            selectedActor = null;
        }
    }

    private void OnEventDrag(PointerEventData eventData)
    {
        HideToolTip();
        var otherSpellID = FindUISpell(eventData);
        if (otherSpellID != null)
        {
            var spellCombineData = GameTableManager.Instance._spellCombineDatas.Find(_ => (_.material_1 == _spellTableData.tableData.spell_id && _.material_2 == otherSpellID._spellTableData.tableData.spell_id));
            spellCombineData ??= GameTableManager.Instance._spellCombineDatas.Find(_ =>_.material_1 == otherSpellID._spellTableData.tableData.spell_id && _.material_2  == _spellTableData.tableData.spell_id );

            if (spellCombineData == null)
                return;

            var resultSpell =
                GameTableManager.Instance._spellData.Find(_ => _.spell_id == spellCombineData.result_spell);
            if (resultSpell == null)
                return;

            if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var ui))
            {
                ui.CreateInfo(resultSpell.spell_name, resultSpell.spell_desc, this.transform.GetComponent<RectTransform>());
                ui.Show();
            }
        }   

        var actor = FindGameActor(eventData);
        if (actor != null)
        {
            if (selectedActor != null)
            {
                selectedActor.OnDeselected();
            }
            selectedActor = actor;
            actor.OnSelected();
        }
        else
        {
            if (selectedActor != null)
            {
                selectedActor.OnDeselected();
                selectedActor = null;
            }
        }
    }

    private UISpell FindUISpell(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var isSpellUI = result.gameObject.GetComponent<UISpell>();
            if (isSpellUI != this && isSpellUI != null && isSpellUI.isActiveAndEnabled)
            {
                return isSpellUI;
            }
        }

        return null;
    }

    private GameActor FindGameActor(PointerEventData eventData)
    {
        if (Camera.main != null)
        {
            var ray = Camera.main.ScreenPointToRay(eventData.position);
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 2f);
            var collideObject = Physics2D.RaycastAll(ray.origin, ray.direction);

            foreach (var hit in collideObject)
            {
                var actor = hit.transform.GetComponent<GameActor>();
                if (actor != null)
                {
                    return actor;
                }
            }
        }

        return null;
    }

    private void ShowToolTip()
    {
        if (GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_4, out var ui))
        {
            ui.CreateInfo(_spellTableData.tableData.spell_name, _spellTableData.tableData.spell_desc,
                this.transform.GetComponent<RectTransform>());
            ui.Show();
        }
    }

    private void HideToolTip()
    {
        if (GameUIManager.Instance.TryGet<UITooltip>(out var ui))
        {
            ui.Hide();
        }
    }
}
