using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

public class UISpell : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private DTButton button;
    private UICardDeckOnHand.SpellData _spellTableData;
    public void SetUI(UICardDeckOnHand.SpellData spellTableData)
    {
        _spellTableData = spellTableData;
        img.sprite = GameResourceManager.Instance.GetImage(spellTableData.tableData.spell_img);
        button.SetHoverEvent(ShowToolTip, HideToolTip);
    }

    private void ShowToolTip()
    {
        if(GameUIManager.Instance.TryGetOrCreate<UITooltip>(true, UILayer.LEVEL_2, out var ui))
        {
            ui.CreateInfo(_spellTableData.tableData.spell_name, _spellTableData.tableData.spell_desc, this.transform as RectTransform);
            ui.Show();
            Debug.Log("OPEN" + GameUtil.GetString(_spellTableData.tableData.spell_name));
        }
    }

    private void HideToolTip()
    {
        if(GameUIManager.Instance.TryGet<UITooltip>(out var ui))
        {
            ui.Hide();
            Debug.Log("CLOSE" +GameUtil.GetString(_spellTableData.tableData.spell_name));
        }
    }
}
